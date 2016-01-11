function [Price, DirtyPrice, CFlowAmounts, CFlowDates] = bondpricing(RateSpec, ...
    CouponRate, Settle, Maturity, varargin)
%BONDBYZERO Price bonds in a portfolio by a set of zero curves.
%
%   Price = bondbyzero(RateSpec, CouponRate, Settle, Maturity)
%
%   Price = bondbyzero(RateSpec, CouponRate, Settle, Maturity,...
%                                Period, Basis, EndMonthRule,   ...
%                                IssueDate, FirstCouponDate, LastCouponDate, ...
%                                StartDate, Face)
%
%   Price = bondbyzero(RateSpec, CouponRate, Settle, Maturity,...
%                      'Param1','Value1',...)
%
%   [Price, DirtyPrice, CFlowAmounts, CFlowDates]  = bondbyzero(RateSpec,
%                   CouponRate, Settle,Maturity,'Param1','Value1',...)
%
%   Required Inputs:
%     All inputs are either scalars or NINST by 1 vectors unless otherwise
%     specified. Dates can be serial date numbers or date strings.
%     Optional arguments can be passed as empty matrices [].
%
%     RateSpec        - The annualized zero rate term structure.
%     CouponRate      - NINSTx1 decimal annual rate or NINSTx1 cell array
%                       where each element is a NumDates x 2 cell array where the
%                       first column is dates and the second column is associated
%                       rates.  The date indicates the last day that the coupon
%                       rate is valid.
%     Settle          - NINSTx1 Settlement date.
%     Maturity        - NINSTx1 Maturity date.
%
%   Optional Inputs:
%     Period          - NINSTx1 Coupons per year. Default is 2.
%     Basis           - NINSTx1 Day-count basis.  Default is 0 (actual/actual).
%     EndMonthRule    - NINSTx1 End-of-month rule.  Default is 1 (in effect).
%     IssueDate       - NINSTx1 Bond issue date.
%     FirstCouponDate - NINSTx1 Irregular first coupon date.
%     LastCouponDate  - NINSTx1 Irregular last coupon date.
%     StartDate       - NINSTx1 Forward starting date of payments.
%     Face            - NINSTx1 of face values or NINSTx1 cell array
%                       where each element is a NumDates x 2 cell array where the
%                       first column is dates and the second column is associated
%                       face value.  The date indicates the last day that the
%                       face value is valid. Default is 100.
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
%     Note that optional inputs can be specified as parameter value pairs.  
%     If AdjustCashFlowsBasis, Holidays or BusinessDayConvention are specified, 
%     optional inputs must be specified as parameter value pairs. Otherwise, 
%     optional inputs may be specified by order according to the help.
%
%   Outputs:
%
%     Price    - NINST by NUMCURVES matrix of clean bond prices.  Each
%                column arises from one of the zero curves.
%
%     DirtyPrice - NINST by NUMCURVES matrix of dirty bond price (clean +
%                 accrued interest). Each column arises from one of the zero 
%                 curves.
%
%     CFlowAmounts - NINST by NUMCFS matrix of cash flows for each bond.
%
%     CFlowDates - NUMCFS by 1 matrix of payment dates for each bond.
%
%
%   Notes: BONDBYZERO computes prices of vanilla bonds, stepped coupon bonds
%          and amortizing bonds.
%
%   See also CFBYZERO, FIXEDBYZERO, FLOATBYZERO, SWAPBYZERO.
%

%   Copyright 1995-2011 The MathWorks, Inc.

% --------------------------------------------------------
% Checking the input arguments
% --------------------------------------------------------
if (nargin < 4)
    error(message('finance:bondbyzero:missingInputs'));
end

% Check first input argument to be term structure
if (nargin<1) || ~isafin(RateSpec, 'RateSpec')
    error(message('finance:bondbyzero:invalidTermStruct'));
end

% Get term structure data
Compounding = intenvget(RateSpec, 'Compounding');
ZeroRates   = intenvget(RateSpec, 'Rates');
ZeroDates   = intenvget(RateSpec, 'EndDates');
ValuationDate = intenvget(RateSpec, 'ValuationDate');

if(isempty(Compounding) || isempty(ZeroRates) || isempty(ZeroDates) || isempty(ValuationDate))
    error(message('finance:bondbyzero:invalidRateSpec'))
end

% Make sure we have serial dates
if(ischar(ZeroDates))
    ZeroDates = datenum(ZeroDates);
end

[RateRows, NumCurves] = size(ZeroRates);
[DateRows, DateCols] = size(ZeroDates);

if RateRows ~= DateRows
    error(message('finance:bondbyzero:mismatchRatesDates'));
end

if DateCols ~= 1
    error(message('finance:bondbyzero:invalidZeroDates'));
end

% Make sure that RateSpec holds zero rates. Intepolate if it
% doesn't.
if(any(ValuationDate ~= intenvget(RateSpec, 'StartDates')))
    RateSpec = intenvset(RateSpec, 'StartDates', ValuationDate);
    ZeroRates   = intenvget(RateSpec, 'Rates');
end

% Check to see whether we have the case of ordered inputs or PV pairs
if ~isempty(varargin) && ischar(varargin{1})
        
    p = inputParser;
    p.addParamValue('principaltype', {'sinking'},...
            @(x) all(ismember(x,{'sinking','bullet'})));
    p.addParamValue('adjustcashflowsbasis', false, @islogical);
    p.addParamValue('holidays', []);
    p.addParamValue('businessdayconvention', {'actual'},...
        @(x) all(ismember(x,{'actual', 'follow', 'previous', 'modifiedfollow',...
        'modifiedprevious'})));
    p.addParamValue('period', []);
    p.addParamValue('basis', []);
    p.addParamValue('endmonthrule', []);
    p.addParamValue('issuedate', []);
    p.addParamValue('firstcoupondate', []);
    p.addParamValue('lastcoupondate', []);
    p.addParamValue('startdate', []);
    p.addParamValue('face',[]);


    try
        p.parse(varargin{:});
    catch ME       
        newMsg = message('finance:bondbyzero:optionalInputError');
        newME = MException(newMsg.Identifier,getString(newMsg));
        newME = addCause(newME, ME);
        throw(newME)
    end

    AdjustCashFlowsBasis = p.Results.adjustcashflowsbasis;
    Holidays =  p.Results.holidays;
    BusinessDayConvention = p.Results.businessdayconvention;
    PrincipalType = p.Results.principaltype;

    [CouponRate, Settle, Maturity, Period, Basis, EndMonthRule, IssueDate, ...
        FirstCouponDate, LastCouponDate, StartDate, Face] = ...
        instargbond(CouponRate, Settle, Maturity, p.Results.period,...
        p.Results.basis, p.Results.endmonthrule, p.Results.issuedate,...
        p.Results.firstcoupondate, p.Results.lastcoupondate,...
        p.Results.startdate, p.Results.face);
else
    AdjustCashFlowsBasis = false;
    Holidays = [];
    BusinessDayConvention = 'actual';
    PrincipalType = 'sinking';

    [CouponRate, Settle, Maturity, Period, Basis, EndMonthRule, IssueDate, ...
        FirstCouponDate, LastCouponDate, StartDate, Face] = ...
        instargbond(CouponRate, Settle, Maturity, varargin{:});
end

% Special rules: Single settlement value
if any(Settle ~= Settle(1))
    error(message('finance:bondbyzero:invalidSettleDate'));
else
    Settle = Settle(1);
end

if(ValuationDate > Settle(1))
    error(message('finance:bondbyzero:invalidValuationDate'))
end


% Create the cash flow amounts, dates, and time factors and rearrange
% into a portfolio of cash flows.
% CFBondDate - NumBonds by NumDates
% AllDates   - NumDates by 1
[CFlowAmounts, CFlowDates, Tpds] = cfamounts(CouponRate, Settle, Maturity, ...
    'Period', Period, 'Basis', Basis, 'EndMonthRule', EndMonthRule, 'IssueDate',...
    IssueDate, 'FirstCouponDate', FirstCouponDate, 'LastCouponDate', LastCouponDate,...
    'StartDate', StartDate, 'Face', Face, 'PrincipalType', PrincipalType, ...
    'AdjustCashFlowsBasis', AdjustCashFlowsBasis, 'Holidays', Holidays,...
    'BusinessDayConvention', BusinessDayConvention, 'CompoundingFrequency', 2,...
    'DiscountBasis', 0);

[CFBondDate, AllDates] = cfport(CFlowAmounts, CFlowDates, Tpds);

% Compute the Discount factors for present value
% NumDates by NumCurves
RS = intenvset(RateSpec, 'StartDates', Settle, 'EndDates', AllDates);
AllDisc = intenvget(RS, 'Disc');

% Present value of the cash flows for each bond (NumBonds by NumCurves)
Price = CFBondDate * AllDisc;

if nargout > 1
    % Calculate the present values of cash flows not including AI
    CFBondDateNoAI = CFBondDate;
    CFBondDateNoAI(:, 1) = 0;
    
    DirtyPrice = CFBondDateNoAI * AllDisc;
end
