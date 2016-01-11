%% price the vanilla swap
Settle = '01-Jan-2016';
Maturity = '01-Jan-2021';
Basis = 2;
Principal = 1e7;
LegRate = [0.014 20]; % [CouponRate Spread]
LegType = [1 0]; % [Fixed Float]
LegReset = [2 2]; % Payments semiannually

discFactor = xlsread('discountFactors.xlsx',1);
zeroRates = -log(discFactor(2:end,2))./discFactor(2:end,1);
rateDates = daysadd(datenum(Settle), round(365.25*discFactor(2:end,1)),0);
RateSpec = intenvset('Rates', zeroRates, 'StartDates',...
    '01-Jan-2016','EndDates', rateDates, 'Compounding', -1);
[Price, SwapRate AI, RecCF, RecCFDates, PayCF, PayCFDates] = ...
    swappricing(RateSpec, LegRate, Settle, Maturity,LegReset, Basis, Principal, LegType);

base_Price = Price;
base_RecCF = RecCF;
base_PayCF = PayCF;
base_NetCF = RecCF - PayCF;

% shock the zero rate 
shockedUp = shockZeroCurve(RateSpec, 25);
[Price, SwapRate AI, RecCF, RecCFDates, PayCF, PayCFDates] = ...
    swappricing(shockedUp, LegRate, Settle, Maturity,LegReset, Basis, Principal, LegType);
up_Price = Price;
up_RecCF = RecCF;
up_PayCF = PayCF;
up_NetCF = RecCF - PayCF;

shockedDown = shockZeroCurve(RateSpec, -25);
[Price, SwapRate AI, RecCF, RecCFDates, PayCF, PayCFDates] = ...
    swappricing(shockedDown, LegRate, Settle, Maturity,LegReset, Basis, Principal, LegType);
dn_Price = Price;
dn_RecCF = RecCF;
dn_PayCF = PayCF;
dn_NetCF = RecCF - PayCF;

dv01 = (up_Price - dn_Price) / 50;
convexity = (up_Price - 2*base_Price + dn_Price) / 625;

% plot the results
figure
subplot(1,2,1)
plot(RecCFDates(2:end-1), dn_PayCF(2:end-1)); hold on;
plot(RecCFDates(2:end-1), base_PayCF(2:end-1));
plot(RecCFDates(2:end-1), up_PayCF(2:end-1)); hold off;
title('Floating Leg Payments with/without Rate Shocks')
legend('Rates Shocked Down', 'Rates Base', 'Rates Shocked Up','Location','SouthEast')

subplot(1,2,2)
plot(RecCFDates, dn_NetCF); hold on;
plot(RecCFDates, base_NetCF);
plot(RecCFDates, up_NetCF); hold off;
title('Net Payments (Fixed-Floating) with/without Rate Shocks')
legend('Rates Shocked Down', 'Rates Base', 'Rates Shocked Up','Location','NorthEast')
hline = refline(0);
hline.Color = 'b';

dateFormat = 1;
datetick('x',dateFormat)

%% price the Treasury bond
Yield = [0.04; 0.05; 0.06];
CouponRate = 0.05;
Settle = '20-Jan-1997';
Maturity = '15-Jun-2002';
Period = 2;
Basis = 0;

[Price, AccruedInt] = bndprice(Yield, CouponRate, Settle,...
Maturity, Period, Basis)