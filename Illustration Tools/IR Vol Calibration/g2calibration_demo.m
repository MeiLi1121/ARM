clear;clc;
% Interest rate inputs
Settle = '2015-11-30';
RateSpec.zeroRates = xlsread('ZeroRates11302015.xlsx', 'IR', 'C3:C62');
TermYrs = 0.5:0.5:30;
RateSpec.rateDates = daysadd(datenum(Settle), round(360*TermYrs),1);
RateSpec.Settle = datenum(Settle);
    
% swaption data
[swaption_data, ~] = xlsread('ZeroRates11302015.xlsx', 'swaption');
swaptions.optionTerms = swaption_data(:,1);
swaptions.swapTerms = swaption_data(:,2);
swaptions.swaptionVols = swaption_data(:,3);

weights = ones(1,size(swaption_data,1));

% G2++ (Hull-White two-factor) model calibration
[G2params, MSE, algo] = g2calibration(RateSpec, swaptions, weights);

% Plot IR Vol Surface and calibrated swaption vol
Exercise = [1:10 12 15 20 25 30];
Maturity = [1:10 12 15 20 25 30];
RateSpec_plot = intenvset('Rates',RateSpec.zeroRates, ...
        'EndDates',RateSpec.rateDates,'StartDate',datenum(Settle), 'Compounding',-1);
[IRVolSurf_implied_full, IRVolSurf_normal_full] = ...
    g2IRVolSurface(RateSpec_plot, G2params, Exercise, Maturity);

% Plot the normal vol surface
[Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
figure
surf(Exercise_m, Maturity_m, IRVolSurf_normal_full');
title('IR Normal Vol Surface by Option Term and Swap Term');
xlabel('Option Term')
ylabel('Swap Term')
zlabel('Normal Vol')
view([90,30,60])
hold on
for i = 1:length(swaptions.optionTerms)
    scatter3(swaptions.optionTerms(i),swaptions.swapTerms(i), ...
        swaptions.swaptionVols(i), 'fill','MarkerEdgeColor','k', ...
        'MarkerFaceColor',[1,0,0])
end
