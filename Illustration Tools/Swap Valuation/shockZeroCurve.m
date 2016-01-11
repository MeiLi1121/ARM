function [shockedRateSpec] = shockZeroCurve(RateSpec, shockSize)

% INPUT
%   RateSpec:   Matlab interest rate object
%   shockSize:  the shock size in bps on zero rates

base_zero_curve = RateSpec.Rates;
shocked_zero_curve = base_zero_curve + shockSize/1e4;

% IR is floored at 0
shocked_zero_curve = max(shocked_zero_curve, 0);

shockedRateSpec = intenvset(RateSpec, 'Rates', shocked_zero_curve);

end

