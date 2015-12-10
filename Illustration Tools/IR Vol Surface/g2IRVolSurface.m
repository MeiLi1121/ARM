function [IRVolSurf_implied, IRVolSurf_normal] = g2IRVolSurface(RateSpec, g2Params, Exercise, Maturity)
%% g2IRVolSurface
%   Calcalate the IR volatility surface by input G2++ parameters and
%   interest rate 

%% Calculate the IR Vol from swaptions
IRVolSurf_implied = zeros(length(Exercise), length(Maturity));
IRVolSurf_normal = zeros(length(Exercise), length(Maturity));
swaptionPriceMatrix = zeros(length(Exercise), length(Maturity));

for exer = 1:length(Exercise)
    for matur = 1:length(Maturity)
        % exercise date of the swaption
        exercise_date = daysadd(RateSpec.StartDates, round(Exercise(exer)*360), 1); 
        
        % maturity date of the swap
        maturity_date = daysadd(exercise_date, round(Maturity(matur)*360), 1); 
        
        % swaption strike
        [~,swaption_strike] = swapbyzero(RateSpec,[NaN 0], RateSpec.StartDates, ...
            maturity_date, 'StartDate', exercise_date,'LegReset',[1 1]);
        
        % calculate the swaption price
        swaptionPriceMatrix(exer,matur) = swaptionbylg2f(RateSpec, ...
            g2Params(1),g2Params(2),g2Params(3),g2Params(4),g2Params(5), ...
            swaption_strike, exercise_date,maturity_date,'Reset',1);
        
        price = swaptionPriceMatrix(exer,matur);
        % calculate the swaption implied vol surface
        G2Vol = @(x) (price - ...
            swaptionbyblk(RateSpec, 'call', swaption_strike, RateSpec.StartDates, ...
                exercise_date, maturity_date, x));
        options = optimset('display','off','MaxFunEvals',100,'TolFun',1e-2,'MaxIter',10);
        try
            IRVolSurf_implied(exer,matur) = lsqnonlin(G2Vol,0.5,0,3,options);
            IRVolSurf_implied(exer,matur) = IRVolSurf_implied(exer,matur)*100;
            IRVolSurf_normal(exer,matur) = IRVolSurf_implied(exer,matur)* ...
                swaption_strike*100;
        catch
            IRVolSurf_implied(exer,matur) = -999;
            IRVolSurf_normal(exer,matur) = -999;
        end
%         IRVolSurf_normal(exer,matur) = fzero(@(x) (swaptionPriceMatrix(exer,matur) - ...
%             swaptionbyblk(RateSpec, 'call', swaption_strike, RateSpec.StartDates, ...
%                 exercise_date, maturity_date, x))^2, 1);
%         IRVolSurf_implied(exer,matur) = blsimpv(100, ...
%             100, 0, Exercise(exer), ...
%             (swaptionPriceMatrix(exer,matur)/swaption_strike)/100);
        
        % calculate the swaption normal vol surface
        
    end
end

end

function SwaptionPrice = swaptionbylg2f(inCurve,a,b,sigma,eta,rho,X,...
    ExerciseDate,Maturity,varargin)
%SWAPTIONBYLG2F Compute swaption price using LG2F model
%
% Syntax:
%
%   Price = swaptionbylg2f(ZeroCurve,a,b,sigma,eta,rho,Strike,ExerciseDate,Maturity)
%   Price = swaptionbylg2f(ZeroCurve,a,b,sigma,eta,rho,Strike,ExerciseDate,Maturity,...
%                           'name1','val1')
%
% Description:
%
%   Compute swaption price for 2 factor additive Gaussian interest rate
%   model given zero curve, a, b, sigma, eta and rho parameters for the following
%   equation
%
%   r(t) = x(t) + y(t) + phi(t)
%   dx(t) = -a*x(t)dt + sigma*dW_1(t), x(0) = 0
%   dy(t) = -b*y(t)dt + eta*dW_2(t), y(0) = 0
%   dW_1(t)*dW_2(t) = rho
%
% Inputs:
%
%   ZeroCurve - IRDataCurve or RateSpec. This is the zero curve that is
%               used to evolve the path of future interest rates.
%
%   a - Mean reversion for 1st factor; specified as a scalar.
%
%   b - Mean reversion for 2nd factor; specified as a scalar.
%
%   sigma - Volatility for 1st factor; specified as a scalar.
%
%   eta - Volatility for 2nd factor; specified as a scalar.
%
%   rho - Scalar correlation of the factors.
%
%   Strike - NumSwaptions x 1 vector of Strike values.
%
%   ExerciseDate - NumSwaptions x 1 vector of serial date numbers or date strings
%                  containing the swaption exercise dates.
%
%   Maturity - NumSwaptions x 1 vector of serial date numbers or date strings
%              containing the swap maturity dates.
%
% Optional Inputs:
%
%   Reset - NumSwaptions x 1 vector of reset frequencies of swaption -- default is 1.
%
%   Notional - NumSwaptions x 1 vector of notional values of swaption -- default is 100.
%
%   OptSpec - NumSwaptions x 1 cell array of strings 'call' or 'put'. A call
%             swaption entitles the buyer to pay the fixed rate. A put
%             swaption entitles the buyer to receive the fixed rate.
%             Default is call.
%
% Example:
%
%   Settle = datenum('15-Dec-2007');
%
%   ZeroTimes = [3/12 6/12 1 5 7 10 20 30]';
%   ZeroRates = [0.033 0.034 0.035 0.040 0.042 0.044 0.048 0.0475]';
%   CurveDates = daysadd(Settle,360*ZeroTimes,1);
%
%   irdc = IRDataCurve('Zero',Settle,CurveDates,ZeroRates);
%
%   a = .07;
%   b = .5;
%   sigma = .01;
%   eta = .006;
%   rho = -.7;
%
%   Reset = 1;
%   ExerciseDate = daysadd(Settle,360*5,1);
%   Maturity = daysadd(ExerciseDate,360*[3;4],1);
%   Strike = .05;
%
%   Price = swaptionbylg2f(irdc,a,b,sigma,eta,rho,Strike,ExerciseDate,Maturity,'Reset',Reset)
%
% Reference:
%
%   [1] Brigo, D and F. Mercurio. Interest Rate Models - Theory and
%   Practice. Springer Finance, 2006.
%
% See also LINEARGAUSSIAN2F

narginchk(9, 15);

if ~isafin(inCurve,'RateSpec') && ~isa(inCurve,'IRDataCurve')
    error(message('fininst:swaptionbylg2f:invalidCurve'));
end

if ~isscalar(a),error(message('fininst:swaptionbylg2f:invalidA')),end
if ~isscalar(b),error(message('fininst:swaptionbylg2f:invalidB')),end
if ~isscalar(sigma),error(message('fininst:swaptionbylg2f:invalidSigma')),end
if ~isscalar(eta),error(message('fininst:swaptionbylg2f:invalidEta')),end
if ~isscalar(rho),error(message('fininst:swaptionbylg2f:invalidRho')),end

ExerciseDate = datenum(ExerciseDate);
Maturity = datenum(Maturity);

if any(ExerciseDate > Maturity)
    error(message('fininst:swaptionbylg2f:MaturityBeforeExercise'));
end

p = inputParser;

p.addParamValue('reset',1);
p.addParamValue('notional',100);
p.addParamValue('optspec',{'call'});

try
    p.parse(varargin{:});
catch ME
    newMsg = message('fininst:swaptionbylg2f:optionalInputError');
    newME = MException(newMsg.Identifier, getString(newMsg));
    newME = addCause(newME, ME);
    throw(newME)
end

Reset = p.Results.reset;
Notional = p.Results.notional;

if ischar(p.Results.optspec)
    OptSpec = cellstr(p.Results.optspec);
elseif iscell(p.Results.optspec)
    OptSpec = p.Results.optspec;
else
    error(message('fininst:swaptionbylg2f:invalidOptSpec'));
end

if ~all(ismember(lower(OptSpec),{'call','put'}))
    error(message('fininst:swaptionbylg2f:invalidOptSpec'));
end

try
    [X, ExerciseDate, Maturity, Reset, Notional, OptSpec] = finargsz(1, X(:), ExerciseDate(:),Maturity(:),...
        Reset(:), Notional(:), OptSpec(:));
catch ME
    throwAsCaller(ME)
end

w = double(strcmpi(OptSpec,'call'));
w(w == 0) = -1;

if isafin(inCurve,'RateSpec')
    Settle = inCurve.ValuationDate;
    PM = @(t) intenvget(intenvset(inCurve,'EndDates',datemnth(Settle, 12*t)),'Disc')';
else
    Settle = inCurve.Settle;
    PM = @(t) inCurve.getDiscountFactors(datemnth(Settle, 12*t))';
end

V = @(t,T) sigma^2/a^2*(T - t + 2/a*exp(-a*(T-t)) - 1/(2*a)*exp(-2*a*(T-t)) - 3/2/a) + ...
    eta^2/b^2*(T - t + 2/b*exp(-b*(T-t)) - 1/(2*b)*exp(-2*b*(T-t)) - 3/2/b) + ...
    2*rho*sigma*eta/(a*b)*(T - t + (exp(-a*(T-t)) - 1)/a + (exp(-b*(T-t)) - 1)/b - ...
    (exp(-(a + b)*(T-t)) - 1)/(a + b));

A = @(t,T) PM(T)./PM(t) .*exp(1/2*(V(t,T) - V(0,T) + V(0,t)));

B = @(z,t,T) (1 - exp(-bsxfun(@times,z,(T-t))))/z;

nSwaptions = length(Maturity);
SwaptionPrice = zeros(nSwaptions,1);

optOptions = optimset('Jacobian','on','display','off');

for swapidx=1:nSwaptions
    
    T = round(yearfrac(Settle,ExerciseDate(swapidx),inCurve.Basis));
    Tenor = round(yearfrac(ExerciseDate(swapidx),Maturity(swapidx),inCurve.Basis));
    
    ti = T:1/Reset(swapidx):(Tenor + T);
    tau = diff(ti);
    c = X(swapidx)*tau;
    c(end) = c(end) + 1;
    ti(1) = [];
    
    ux = -(sigma^2/a^2 + rho*sigma*eta/a/b)*(1 - exp(-a*T)) + ...
        sigma^2/(2*a^2)*(1 - exp(-2*a*T)) + ...
        rho*sigma*eta/(b*(a+b))*(1 - exp(-b*T - a*T));
    
    uy = -(eta^2/b^2 + rho*sigma*eta/a/b)*(1 - exp(-b*T)) + ...
        eta^2/(2*b^2)*(1 - exp(-2*b*T)) + ...
        rho*sigma*eta/(a*(a+b))*(1 - exp(-b*T - a*T));
    
    sigx = sigma*sqrt((1-exp(-2*a*T))/2/a);
    sigy = eta*sqrt((1-exp(-2*b*T))/2/b);
    rhoxy = rho*sigma*eta/((a+b)*sigx*sigy)*(1-exp(-(a+b)*T));
    
    x = linspace(ux - 10*sigx,ux + 10*sigx,1001)';
    
    cA = c.*A(T,ti);
    [ybar,~,exitflag] = fsolve(@(ybar) localObjFcn(ybar,x,cA,...
        B(a,T,ti),B(b,T,ti)),-x,optOptions);
    
    if exitflag <= 0
        error(message('fininst:swaptionbylg2f:rootFailure'));
    end
    
    h1 = (ybar - uy)./(sigy*sqrt(1 - rhoxy^2)) - rhoxy*(x - ux)./(sigx*sqrt(1 - rhoxy^2));
    h2 = bsxfun(@plus,B(b,T,ti).*sigy*sqrt(1 - rhoxy^2),h1);
    
    lambda = bsxfun(@times,A(T,ti).*c,exp(-bsxfun(@times,B(a,T,ti),x)));
    
    k = bsxfun(@times,-B(b,T,ti),bsxfun(@plus,uy - .5*(1 - rhoxy.^2)*sigy^2*B(b,T,ti),...
        rhoxy*sigy*(x-ux)/sigx));
    
    Y = exp(-1/2*((x - ux)./sigx).^2)./(sigx*sqrt(2*pi)) .* ...
        (normcdf(-w(swapidx)*h1) - sum(lambda.*exp(k).*normcdf(-w(swapidx)*h2),2));
    
    TempVal = trapz(x,Y);
    
    SwaptionPrice(swapidx) = w(swapidx)*Notional(swapidx)*TempVal*PM(T);
end
end

function [f,g] = localObjFcn(y,x,cA,Ba,Bb)
% LOCALOBJFUN Local function for solving roots of equation

tmpSum = bsxfun(@times,cA,exp(-bsxfun(@times,Ba,x) - bsxfun(@times,Bb,y)));
f = sum(tmpSum,2) - 1;
g = diag(-sum(bsxfun(@times,Bb,tmpSum),2));
end