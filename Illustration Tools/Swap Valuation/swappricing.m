function [Price, SwapRate, AI, RecCF, RecCFDates, PayCF, PayCFDates] = ...
    swappricing(RateSpec, LegRate, Settle, Maturity,varargin)
%SWAPBYZERO Price a vanilla swap by a set of zero curve(s).
%
%   [Price, SwapRate] = swapbyzero(RateSpec, LegRate, Settle, Maturity)
%
%   [Price, SwapRate] = swapbyzero(RateSpec, LegRate, Settle, Maturity,...
%                                  LegReset, Basis,  Principal, LegType, EndMonthRule)
%
%   [Price, SwapRate] = swapbyzero(RateSpec, LegRate, Settle, Maturity,...
%                                  'Param1','Value1',...)
%
%   [Price, SwapRate, AI, RecCF, RecCFDates, PayCF, PayCFDates] = ...
%                   swapbyzero(RateSpec, LegRate, Settle, Maturity,...
%                                  'Param1','Value1',...)
%
%   Required Inputs:
%     Dates can be serial date numbers or date strings.
%     Optional arguments can be passed as empty matrices [].
%
%     RateSpec - The annualized zero rate term structure.
%
%                RateSpec can also be a 1-by-2 input variable of RateSpecs, 
%                with the second RateSpec structure containing the discount 
%                curve(s) for the paying leg. If only one RateSpec structure
%                is specified, then this RateSpec is used to discount both legs. 
%
%     LegRate - NINSTx2 matrix, with each row defined as follows:
%               [CouponRate Spread] or [Spread CouponRate]
%               where CouponRate is the decimal annual rate and Spread is
%               the number of basis points over the annualized zero curve.
%               The first column represents the receiving leg, while the
%               second column represents the paying leg.
%
%     Settle - NINSTx1 vector of dates representing the settle date for each
%              swap. 
%
%     Maturity - NINSTx1 vector of dates representing the maturity date for
%                each swap.
%
%   Optional Inputs:
%   LegReset - NINSTx2 matrix representing the reset frequency per year for
%                each swap. Default is [1 1].
%
%   Basis    - NINSTx1 vector of day-count basis. Default is 0 (actual/actual).
%
%   Principal  - NINSTx1 of notional principal amounts or NINSTx1 cell array
%                where each element is a NumDates x 2 cell array where the
%                first column is dates and the second column is associated
%                principal amount.  The date indicates the last day that the
%                principal value is valid. Default is 100.
%
%   LegType - NINSTx2 matrix, with each row representing an instrument, and
%             each column indicating if the corresponding leg is fixed or floating.
%             A value of 0 represents a floating leg, and a value of 1 represents
%             a fixed leg. Use this matrix to define how to interpret the values
%             entered in the Matrix LegRate. Default is [1,0] for each instrument.
%
%   EndMonthRule - NINSTx1 vector representing End-of-month rule.
%                  Default is 1 (in effect).
%
%   LatestFloatingRate - Latest floating rate used for the floating leg
%                        side of the swap.  If this is not specified,
%                        then the latest floating rate is computed from the RateSpec.
%
%   ForwardRateSpec - Forward rate spec to be used in generating cash
%                     flows for floating leg of the swap - if this is not
%                     specified than the RateSpec is used both for discounting
%                     cash flows and generating floating cash flows.
%
%   AdjustCashFlowsBasis - NINSTx1 vector of logicals. Adjusts cash flows according
%                          to the accrual amount. Default is false.
%
%   Holidays - NHOLIDAYSx1 vector of MATLAB date numbers. Holidays to be used in
%              computing business days. Default is to use the holidays in
%              holidays.m.
%
%   BusinessDayConvention - NINSTx1 cell array of business day convention
%                           to be used in computing payment dates - possible
%                           values are actual (default), follow, modifiedfollow,
%                           previous, modifiedprevious.
%
%   StartDate - NINSTx1 vector of dates when the swaps actually start. 
%               Default is Settle.
%
%     Note that optional inputs can be specified as parameter value pairs.
%     If LatestFloatingRate, ForwardRateSpec, AdjustCashFlowsBasis, Holidays, 
%     BusinessDayConvention or StartDate are specified, optional inputs must 
%     be specified as parameter value pairs. Otherwise, optional inputs may 
%     be specified by order according to the help.
%
%   Outputs:
%     Price - NINST by NUMCURVES matrix of swap prices.
%             Each column arises from one of the zero curves.
%
%     SwapRate - NINST by NUMCURVES matrix of rates applicable to the fixed leg
%                such that the swaps' values are zero at Settle. This rate is
%                calculated and used in calculating the swaps prices when
%                the rate specified for the fixed leg in LegRate is NaN.
%                SwapRate is padded with NaNs for those
%                instruments in which the coupon rate is not set to NaN.
%
%     AI - NINST by NUMCURVES matrix of accrued interest.
%
%     RecCF - NINST by NUMCURVES matrix of cash flows for the receiving leg
%     Note that if there is more than 1 curve specified in the RateSpec
%     input, then the first NCURVES rows correspond to the first bond,
%     the second NCURVES rows correspond to the second bond and so on.
%
%     RecCFDates - NINST by NUMCURVES matrix of payment dates for the receiving leg
%
%     PayCF - NINST by NUMCURVES matrix of cash flows for the paying leg
%
%     PayCFDates - NINST by NUMCURVES matrix of payment dates for the paying leg
%
% Notes: 
%        This function also calculates the SwapRate (fixed rate) so that the
%        value of the Swap is initially zero. To do this CouponRate should be
%        entered as NaN.
%
%        If a 1-by-2 input vector of RateSpecs is specified, the LegType of  
%        all swaps should be the same.
%
%        SWAPBYZERO computes prices of vanilla swaps, amortizing swaps and
%        forward swaps.

%   See also BONDBYZERO, CFBYZERO, FIXEDBYZERO, FLOATBYZERO.

%   Copyright 1995-2012 The MathWorks, Inc.
%   $Revision: 1.8.2.23 $  $Date: 2014/05/17 14:35:28 $

% -------------------------------------------------------------
% Checking the input arguments
% -------------------------------------------------------------
if (nargin < 4)
    error(message('finance:swapbyzero:missingInputs'));
end

% Check to see whether we have the case of ordered inputs or PV pairs
if ~isempty(varargin) && ischar(varargin{1})
    
    p = inputParser;
    
    p.addParamValue('latestfloatingrate',NaN,@isnumeric);
    p.addParamValue('adjustcashflowsbasis',false,@islogical);
    p.addParamValue('holidays',[]);
    p.addParamValue('forwardratespec',[]);
    p.addParamValue('projectioncurve',[]);
    p.addParamValue('businessdayconvention',{'actual'},...
        @(x) all(ismember(x,{'actual','follow','previous','modifiedfollow',...
        'modifiedprevious'})))
    p.addParamValue('legreset',[]);
    p.addParamValue('basis',[]);
    p.addParamValue('principal',[]);
    p.addParamValue('legtype', []);
    p.addParamValue('endmonthrule', []);
    p.addParamValue('startdate', []);
    
    try
        p.parse(varargin{:});
    catch ME
        newMsg = message('finance:swapbyzero:optionalInputError');
        newME = MException(newMsg.Identifier,getString(newMsg));
        newME = addCause(newME,ME);
        throw(newME)
    end
    
    LatestFloatingRate = p.Results.latestfloatingrate;
    AdjustCashFlowsBasis = p.Results.adjustcashflowsbasis;
    Holidays =  p.Results.holidays;
    ForwardRateSpec = p.Results.forwardratespec;
    ProjCurve = p.Results.projectioncurve;
    BusinessDayConvention = p.Results.businessdayconvention;
    
    if isempty(ForwardRateSpec)
        ForwardRateSpec = ProjCurve;
    end
    
    [LegRate, Settle, Maturity, LegReset, Basis, Principal, LegType, EOM, StartDate] = ...
        instargswap(LegRate, Settle, Maturity,p.Results.legreset,...
        p.Results.basis,p.Results.principal,p.Results.legtype,p.Results.endmonthrule, p.Results.startdate);
    
    [LegRate, Settle, Maturity, LegReset, Basis, Principal, LegType,...
        EOM,LatestFloatingRate,AdjustCashFlowsBasis,BusinessDayConvention, StartDate] = ...
        finargsz(1, LegRate, Settle, Maturity, LegReset, Basis, Principal, LegType,...
        EOM,LatestFloatingRate,AdjustCashFlowsBasis,BusinessDayConvention, StartDate);
else
    LatestFloatingRate = NaN;
    AdjustCashFlowsBasis = false;
    Holidays =  [];
    ForwardRateSpec = [];
    BusinessDayConvention = 'actual';
    [LegRate, Settle, Maturity, LegReset, Basis, Principal, LegType, EOM, StartDate] = ...
        instargswap(LegRate, Settle, Maturity, varargin{:});
end

% Special rules: Single settlement value
if any(Settle ~= Settle(1))
    error(message('finance:swapbyzero:invalidSettleDate'));
end
Settle = Settle(1);

% -------------------------------------------------------------
% Separate floating notes from fixed notes
% -------------------------------------------------------------
[NumInst, NumCols] = size(LegType);

% Find row/col & linear indicies of each leg (floating & fixed).  We sort
% them all based on row order to maintain the "instrument order" of the
% swaps.  This is necessary to keep the function P/V pairs associated with
% their respective instruments.
floatInd = find(LegType == 0);
[floatRows,floatCols] = ind2sub([NumInst, NumCols], floatInd);
[floatRows,sortedRowInds] = sort(floatRows);
floatCols = floatCols(sortedRowInds);
floatInd = floatInd(sortedRowInds);

fixedInd = find(LegType == 1);
[fixedRows,fixedCols] = ind2sub([NumInst, NumCols], fixedInd);
[fixedRows,sortedRowInds] = sort(fixedRows);
fixedCols = fixedCols(sortedRowInds);
fixedInd = fixedInd(sortedRowInds);

% Break apart arguments into floating & fixed arrays
Spreads = LegRate(floatInd);
FixedRates  = LegRate(fixedInd);

FloatResets = LegReset(floatInd);
FixedResets = LegReset(fixedInd);

% Check to see if Basis is just a column
if size(Basis,2) == 1
    Bas = repmat(Basis, 1, NumCols);
else
    Bas = Basis;
end
FloatBasis = Bas(floatInd);
FixedBasis = Bas(fixedInd);

if length(RateSpec) == 1
    if ~isafin(RateSpec,'RateSpec')
        error(message('finance:swapbyzero:invalidTermStruct'));
    else
        FloatRateSpec = RateSpec;
        FixedRateSpec = RateSpec;
        ZeroRates   = intenvget(RateSpec, 'Rates');
        [~, NumCurves] = size(ZeroRates);
        ValuationDate = intenvget(RateSpec, 'ValuationDate');
    end
elseif length(RateSpec) == 2
    if ~isafin(RateSpec(1),'RateSpec') || ~isafin(RateSpec(2),'RateSpec')
        error(message('finance:swapbyzero:invalidTermStructs'));
    else
        % Check to confirm that all the swaps are of the same type
        if any(any(diff(LegType,[],1) ~= 0))
            error(message('finance:swapbyzero:invalidTermStructandLeg'));
        else
            if LegType(1) == 1
                FloatRateSpec = RateSpec(2);
                FixedRateSpec = RateSpec(1);
            else
                FloatRateSpec = RateSpec(1);
                FixedRateSpec = RateSpec(2);
            end
            % Only 1 curve per RateSpec if we need different curve for both
            NumCurves = 1;
            if intenvget(FloatRateSpec, 'ValuationDate') ~= intenvget(FixedRateSpec, 'ValuationDate')
                error(message('finance:swapbyzero:invalidValuationDates'));
            else
                ValuationDate = intenvget(FloatRateSpec, 'ValuationDate');
            end
        end
    end
else
    error(message('finance:swapbyzero:invalidTermStruct2'));
end

if ValuationDate > Settle
    error(message('finance:swapbyzero:invalidValuationDate'))
end

% -------------------------------------------------------------
% If any of the coupon rates is a NaN, we need to calculate the
% coupon rates. Find their location for later processing
SRFixedInd = [];
if any(isnan(FixedRates))
    [SRFixedInd, SRFloatInd] = findSwapRateInds(LegRate, LegType, floatInd, fixedInd);
end

% --------------------------------------------------------------
% Find prices for each one of the instruments
% --------------------------------------------------------------
[FloatCleanPrice, FloatDirtyPrice,FloatCF,FloatCFDates] = floatbyzero(FloatRateSpec, Spreads, Settle,...
    Maturity, 'Reset',FloatResets, 'Basis',FloatBasis, 'Principal',Principal,...
    'EndMonthRule',EOM,'LatestFloatingRate',LatestFloatingRate,...
    'AdjustCashFlowsBasis',AdjustCashFlowsBasis,'Holidays',Holidays,...
    'BusinessDayConvention',BusinessDayConvention,'ForwardRateSpec',ForwardRateSpec, ...
    'StartDate', StartDate);

% --------------------------------------------------------------
% If any of the fixed legs were set to NaN, we must calculate the
% corresponding SwapRate
if any(isnan(FixedRates))
    SwapRates = findswaprates(FixedRateSpec, FloatDirtyPrice(SRFloatInd, :), ...
        Settle, Principal(SRFixedInd), Maturity(SRFixedInd),...
        FixedResets(SRFixedInd), FixedBasis(SRFixedInd), EOM(SRFixedInd), 'StartDate', StartDate(SRFixedInd));
    
    FixedRates(SRFixedInd) = SwapRates;
end

[FixedCleanPrice, FixedDirtyPrice, FixedCF,FixedCFDates] = fixedbyzero(FixedRateSpec, FixedRates, ...
    Settle, Maturity,'Reset',FixedResets, 'Basis',FixedBasis, 'Principal',Principal,...
    'EndMonthRule',EOM,'AdjustCashFlowsBasis',AdjustCashFlowsBasis,'Holidays',Holidays,...
    'BusinessDayConvention',BusinessDayConvention, 'StartDate', StartDate);

% --------------------------------------------------------
% Find the Price of each swap
% --------------------------------------------------------

FloatRcvIdx = floatRows(floatCols == 1);
FloatPayIdx = floatRows(floatCols == 2);
FixedRcvIdx = fixedRows(fixedCols == 1);
FixedPayIdx = fixedRows(fixedCols == 2);

LValsDirty = NaN*ones(NumInst,NumCurves);
RValsDirty = LValsDirty;

LogLegType = logical(LegType);
FixedRcvMask =  LogLegType(:,1);
FloatRcvMask = ~LogLegType(:,1);
FixedPayMask =  LogLegType(:,2);
FloatPayMask = ~LogLegType(:,2);

LValsDirty(FixedRcvMask,:) = FixedDirtyPrice(FixedRcvIdx,:);
LValsDirty(FloatRcvMask,:) = FloatDirtyPrice(FloatRcvIdx,:);

RValsDirty(FixedPayMask,:) = FixedDirtyPrice(FixedPayIdx,:);
RValsDirty(FloatPayMask,:) = FloatDirtyPrice(FloatPayIdx,:);

% Price output is dirty price
Price =  LValsDirty - RValsDirty;

% --------------------------------------------------------
% If requested, return the SwapRate where applicable
% --------------------------------------------------------
if nargout >= 2
    SwapRate = NaN*ones(NumInst, NumCurves);
    if ~isempty(SRFixedInd)
        SwapRate(fixedRows(SRFixedInd), :) = SwapRates;
    end
end

if nargout >= 3
    LValClean = NaN*ones(NumInst,NumCurves);
    RValClean = LValClean;
    
    LValClean(FixedRcvMask,:) = FixedCleanPrice(FixedRcvIdx,:);
    LValClean(FloatRcvMask,:) = FloatCleanPrice(FloatRcvIdx,:);
    
    RValClean(FloatPayMask,:) = FloatCleanPrice(FloatPayIdx,:);
    RValClean(FixedPayMask,:) = FixedCleanPrice(FixedPayIdx,:);
    
    CleanPrice =  LValClean - RValClean;       
    AI = Price - CleanPrice;
end
if nargout >= 4
    RecCF(FloatRcvMask,:) = FloatCF(FloatRcvIdx,:);
    tmpFixedCF = FixedCF(FixedRcvIdx,:);
    if ~isempty(tmpFixedCF)
        RecCF(FixedRcvMask,1:size(tmpFixedCF,2)) = tmpFixedCF;
    end
    
    RecCFDates(FloatRcvMask,:) = FloatCFDates(FloatRcvIdx,:);
    tmpFixedCFDates = FixedCFDates(FixedRcvIdx,:);
    if ~isempty(tmpFixedCFDates)
        RecCFDates(FixedRcvMask,1:size(tmpFixedCFDates,2)) = tmpFixedCFDates;
    end
    
    PayCF(FloatPayMask,:) = FloatCF(FloatPayIdx,:);
    tmpFixedCF2 = FixedCF(FixedPayIdx,:);
    if ~isempty(tmpFixedCF2)
        PayCF(FixedPayMask,1:size(tmpFixedCF2,2)) = tmpFixedCF2;
    end
    
    PayCFDates(FloatPayMask,:) = FloatCFDates(FloatPayIdx,:);
    tmpFixedCFDates2 = FixedCFDates(FixedPayIdx,:);
    if ~isempty(tmpFixedCFDates2)
        PayCFDates(FixedPayMask,1:size(tmpFixedCFDates2,2)) = tmpFixedCFDates2;
    end
end

% ---------------------------------------------------------
% Aux function to find the location of the instruments needed to
% calculate the SwapRates
function [SRFixedInd, SRFloatInd] = findSwapRateInds(LegRate, LegType, floatInd, fixedInd)
NumInst = size(LegRate,1);
% ---------------------------------------------------------
% If any of the coupon rates was entered as NaN, we must
% substitute it by the corresponding swap rate.
% ---------------------------------------------------------
[SwapRateRowInd, ~] = find(isnan(LegRate));

% -------------------------------------------------------------
% Identify Swap Rate rows. Sister Float and Fixed Rate Notes in
% swaps will share the same rows.
% -------------------------------------------------------------
SwapRateRowMask = zeros(size(LegRate));
SwapRateRowMask(SwapRateRowInd,:) = 1;

SwapFloatInd = find(SwapRateRowMask & (LegType == 0));
SwapFixedInd = NaN*ones(size(SwapFloatInd));
SwapFixedInd(SwapFloatInd>NumInst)  = SwapFloatInd(SwapFloatInd>NumInst)-NumInst;
SwapFixedInd(SwapFloatInd<=NumInst) = SwapFloatInd(SwapFloatInd<=NumInst)+NumInst;

SRFloatInd = findSupersetIndex(floatInd, SwapFloatInd);
SRFixedInd = findSupersetIndex(fixedInd, SwapFixedInd);

function result = findSupersetIndex(Superset, Subset)
% for each element in the subset, find the index in the superset array

result = NaN(size(Subset));
for i = 1:numel(Subset)
    ind = find(Superset == Subset(i),1);
    if ~isempty(ind)
        result(i) = ind;
    end
end



