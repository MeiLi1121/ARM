function [G2params, MSE, algo] = g2calibration(IRstruct, swaptions, weights)
% G2CALIBRATION
%   Function returns G2++ parameters and mean squared error given swap
%   curves, swaption normal vols and weights on different swaption points

% INPUT
%   IRstruct:       IR data structure that contains the settle data and
%                   zero rates
%   swaptionVols:   the terms and normal volatilities of swaptions
%   weights:        the weights matrix that determines the 

% OUTPUT
%   G2params:   5 G2++ parameters structure with parameter name and values
%   MSE:        the mean squared error of the calibrated swaption vols 
%   algo:       the optimization algorithm finally got used

    % Construct the RateSpec
    RateSpec = ...
        intenvset('Rates',IRstruct.zeroRates, ...
        'EndDates',IRstruct.rateDates,'StartDate',IRstruct.Settle, 'Compounding',-1);
    
    exercise_dates = daysadd(IRstruct.Settle, round(360*swaptions.optionTerms), 1);
    swap_tenors = daysadd(exercise_dates, round(360*swaptions.swapTerms), 1);
    
    swaption_prices_for_calibration = zeros(length(exercise_dates), 1);
    weights = sqrt(weights);
    swaption_strike = zeros(length(exercise_dates), 1);
    
    for swpind = 1:length(exercise_dates)
        [~,swaption_strike(swpind)] = swapbyzero(RateSpec,[NaN 0], IRstruct.Settle, swap_tenors(swpind),...
                'StartDate',exercise_dates(swpind),'LegReset',[1 1]);
        swaption_black_vol_for_calibration = ...
                ((swaptions.swaptionVols(swpind)/ ...
                swaption_strike(swpind)/10000));    
        swaption_prices_for_calibration(swpind) = ...
            swaptionbyblk(RateSpec, 'call', swaption_strike(swpind), IRstruct.Settle, ...
                exercise_dates(swpind), swap_tenors(swpind), swaption_black_vol_for_calibration);
    end
    
    G2PPobjfun = @(x) (swaption_prices_for_calibration...
        -swaptionbylg2f(RateSpec,x(1),x(2),x(3),x(4),x(5),swaption_strike,...
        exercise_dates,swap_tenors,'Reset',1))'.* weights;
    try
        % GA solver
        x0 = [0.2 0.2 0.1 0.1 -0.5];
        options = gaoptimset('InitialPopulation', x0, 'Display', ...
            'iter', 'TolFun', 1e-6, 'Generations', 150);
        lb = [1e-6 1e-6 1e-6 1e-6 -0.8];
        ub = [10 10 10 10 0.8];
        G2params = ga(G2PPobjfun,5,[],[],[],[],lb,ub,[],[],options);
        algo = 2;
    catch
        try
            % local solver
            options = optimset('disp','iter','MaxFunEvals',10000,'TolFun',...
                1e-6,'MaxIter',150);
            x0 = [0.2 0.2 0.1 0.1 -0.5];
            lb = [1e-6 1e-6 1e-6 1e-6 -0.8];
            ub = [10 10 1 1 0.8];
            G2params = lsqnonlin(G2PPobjfun,x0,lb,ub,options);
            algo = 1;
        catch
            G2params = zeros(5,1);
            algo = 0;
        end
    end
    
    orig_prices = swaption_prices_for_calibration;
    calibrated_prices = swaptionbylg2f(RateSpec,G2params(1), ...
        G2params(2),G2params(3), ...
        G2params(4),G2params(5), ...
        swaption_strike(:), exercise_dates,swap_tenors,'Reset',1);
    MSE = norm((orig_prices-calibrated_prices), 2);
    
   
end