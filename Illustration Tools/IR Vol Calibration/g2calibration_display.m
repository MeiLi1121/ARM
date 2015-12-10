function varargout = g2calibration_display(varargin)
% G2CALIBRATION_DISPLAY MATLAB code for g2calibration_display.fig
%      G2CALIBRATION_DISPLAY, by itself, creates a new G2CALIBRATION_DISPLAY or raises the existing
%      singleton*.
%
%      H = G2CALIBRATION_DISPLAY returns the handle to a new G2CALIBRATION_DISPLAY or the handle to
%      the existing singleton*.
%
%      G2CALIBRATION_DISPLAY('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in G2CALIBRATION_DISPLAY.M with the given input arguments.
%
%      G2CALIBRATION_DISPLAY('Property','Value',...) creates a new G2CALIBRATION_DISPLAY or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before g2calibration_display_OpeningFcn gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to g2calibration_display_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Edit the above text to modify the response to help g2calibration_display

% Last Modified by GUIDE v2.5 09-Dec-2015 19:54:16

% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @g2calibration_display_OpeningFcn, ...
                   'gui_OutputFcn',  @g2calibration_display_OutputFcn, ...
                   'gui_LayoutFcn',  [] , ...
                   'gui_Callback',   []);
if nargin && ischar(varargin{1})
    gui_State.gui_Callback = str2func(varargin{1});
end

if nargout
    [varargout{1:nargout}] = gui_mainfcn(gui_State, varargin{:});
else
    gui_mainfcn(gui_State, varargin{:});
end
% End initialization code - DO NOT EDIT


% --- Executes just before g2calibration_display is made visible.
function g2calibration_display_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to g2calibration_display (see VARARGIN)

% Choose default command line output for g2calibration_display
handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

% UIWAIT makes g2calibration_display wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = g2calibration_display_OutputFcn(hObject, eventdata, handles) 
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;


% --- Executes on button press in pushbuttonCalibration.
function pushbuttonCalibration_Callback(hObject, eventdata, handles)
% hObject    handle to pushbuttonCalibration (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

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
[~, IRVolSurf_normal_full] = ...
    g2IRVolSurface(RateSpec_plot, G2params, Exercise, Maturity);

% Plot the normal vol surface
[Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
surf(Exercise_m, Maturity_m, IRVolSurf_normal_full');
title('IR Normal Vol Surface and Original Normal Vols');
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

set(handles.textMeanRevDisplay1, 'String', G2params(1));
set(handles.textMeanRevDisplay2, 'String', G2params(2));
set(handles.textVolDisplay1, 'String', G2params(3));
set(handles.textVolDisplay1, 'String', G2params(4));
set(handles.textCorrDisplay, 'String', G2params(5));
drawnow

% Choose default command line output for IR_Vol
handles.G2params = G2params;

% Update handles structure
guidata(hObject, handles);
