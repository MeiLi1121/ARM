clear;clc;
% Interest rate inputs
Settle = '2015-11-30';
zeroRates = xlsread('ZeroRates11302015.xlsx', 'IR', 'C3:C62');

% G2++ parameters are in order of:
% Mean-reverting 1, mean-reverting 2, volatility 1, volatility 2, correlation
G2Parameters = [0.72180000, 0.03780000, 0.01839520, 0.01396646, -0.80000000];

tic

% Model inputs
TermYrs = 0.5:0.5:30;
rateDates = daysadd(datenum(Settle), round(360*TermYrs),1);
RateSpec = intenvset('Rates',zeroRates, ...
        'EndDates',rateDates,'StartDate',datenum(Settle), 'Compounding',-1);
Exercise = [1:10 12 15 20 25 30];
Maturity = [1:10 12 15 20 25 30];
    
% swaption pricing
exercise_term = 5; swap_term = 10; % price a 5 into 10 swaption
exercise_date = daysadd(RateSpec.StartDates, round(exercise_term*360), 1); 
maturity_date = daysadd(exercise_date, round(swap_term*360), 1); 
[~,swaption_strike] = swapbyzero(RateSpec,[NaN 0], RateSpec.StartDates, ...
    maturity_date, 'StartDate', exercise_date,'LegReset',[1 1]);

swaption_price_g2pp = swaptionbylg2f(RateSpec, ...
    G2Parameters(1), G2Parameters(2),G2Parameters(3),G2Parameters(4),G2Parameters(5),...
            swaption_strike, exercise_date, maturity_date,'Reset',1);
% swaption_price_black = swaptionbyblk(RateSpec, 'call', swaption_strike, RateSpec.StartDates, ...
%                 exercise_date, maturity_date, x);

% Model output
[IRVolSurf_implied_full, IRVolSurf_normal_full] = ...
    g2IRVolSurface(RateSpec, G2Parameters, Exercise, Maturity);

% Numerically instability
IRVolSurf_normal_full(IRVolSurf_normal_full == -999) = nan;
IRVolSurf_implied_full(IRVolSurf_implied_full == -999) = nan;


% Plot the normal vol surface
[Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
figure
subplot(1,2,1)
surf(Exercise_m, Maturity_m, IRVolSurf_normal_full');
title('IR Normal Vol Surface by Option Term and Swap Term');
xlabel('Option Term')
ylabel('Swap Term')
zlabel('Normal Vol')
view([90,30,60])

subplot(1,2,2)
surf(Exercise_m, Maturity_m, IRVolSurf_implied_full');
title('IR Implied Vol Surface by Option Term and Swap Term');
xlabel('Option Term')
ylabel('Swap Term')
zlabel('Implied Vol')
view([90,30,60])

toc