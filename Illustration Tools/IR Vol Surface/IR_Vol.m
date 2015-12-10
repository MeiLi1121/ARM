function varargout = IR_Vol(varargin)
% IR_VOL MATLAB code for IR_Vol.fig
%      IR_VOL, by itself, creates a new IR_VOL or raises the existing
%      singleton*.
%
%      H = IR_VOL returns the handle to a new IR_VOL or the handle to
%      the existing singleton*.
%
%      IR_VOL('CALLBACK',hObject,eventData,handles,...) calls the local
%      function named CALLBACK in IR_VOL.M with the given input arguments.
%
%      IR_VOL('Property','Value',...) creates a new IR_VOL or raises the
%      existing singleton*.  Starting from the left, property value pairs are
%      applied to the GUI before IR_Vol_OpeningFcn gets called.  An
%      unrecognized property name or invalid value makes property application
%      stop.  All inputs are passed to IR_Vol_OpeningFcn via varargin.
%
%      *See GUI Options on GUIDE's Tools menu.  Choose "GUI allows only one
%      instance to run (singleton)".
%
% See also: GUIDE, GUIDATA, GUIHANDLES

% Edit the above text to modify the response to help IR_Vol

% Last Modified by GUIDE v2.5 05-Dec-2015 14:54:27

% Begin initialization code - DO NOT EDIT
gui_Singleton = 1;
gui_State = struct('gui_Name',       mfilename, ...
                   'gui_Singleton',  gui_Singleton, ...
                   'gui_OpeningFcn', @IR_Vol_OpeningFcn, ...
                   'gui_OutputFcn',  @IR_Vol_OutputFcn, ...
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


% --- Executes just before IR_Vol is made visible.
function IR_Vol_OpeningFcn(hObject, eventdata, handles, varargin)
% This function has no output args, see OutputFcn.
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)
% varargin   command line arguments to IR_Vol (see VARARGIN)

% Choose default command line output for IR_Vol
handles.output = hObject;

% Update handles structure
guidata(hObject, handles);

% UIWAIT makes IR_Vol wait for user response (see UIRESUME)
% uiwait(handles.figure1);


% --- Outputs from this function are returned to the command line.
function varargout = IR_Vol_OutputFcn(hObject, eventdata, handles) 
% varargout  cell array for returning output args (see VARARGOUT);
% hObject    handle to figure
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Get default command line output from handles structure
varargout{1} = handles.output;


% --- Executes on slider movement.
function sliderMeanRevertFactor1_Callback(hObject, eventdata, handles)
% hObject    handle to sliderMeanRevertFactor1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'Value') returns position of slider
%        get(hObject,'Min') and get(hObject,'Max') to determine range of slider
mr1 = get(handles.sliderMeanRevertFactor1, 'value');
set(handles.editMeanReverting1, 'String', num2str(mr1));
drawnow
guidata(hObject, handles);

% --- Executes during object creation, after setting all properties.
function sliderMeanRevertFactor1_CreateFcn(hObject, eventdata, handles)
% hObject    handle to sliderMeanRevertFactor1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: slider controls usually have a light gray background.
if isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor',[.9 .9 .9]);
end


% --- Executes on slider movement.
function sliderMeanRevertFactor2_Callback(hObject, eventdata, handles)
% hObject    handle to sliderMeanRevertFactor2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'Value') returns position of slider
%        get(hObject,'Min') and get(hObject,'Max') to determine range of slider
mr2 = get(handles.sliderMeanRevertFactor2, 'value');
set(handles.editMeanReverting2, 'String', num2str(mr2));
drawnow
guidata(hObject, handles);

% --- Executes during object creation, after setting all properties.
function sliderMeanRevertFactor2_CreateFcn(hObject, eventdata, handles)
% hObject    handle to sliderMeanRevertFactor2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: slider controls usually have a light gray background.
if isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor',[.9 .9 .9]);
end


% --- Executes on slider movement.
function sliderVolFactor1_Callback(hObject, eventdata, handles)
% hObject    handle to sliderVolFactor1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'Value') returns position of slider
%        get(hObject,'Min') and get(hObject,'Max') to determine range of slider
vol1 = get(handles.sliderVolFactor1, 'value');
set(handles.editVol1, 'String', num2str(vol1));
drawnow
guidata(hObject, handles);

% --- Executes during object creation, after setting all properties.
function sliderVolFactor1_CreateFcn(hObject, eventdata, handles)
% hObject    handle to sliderVolFactor1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: slider controls usually have a light gray background.
if isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor',[.9 .9 .9]);
end


% --- Executes on slider movement.
function sliderVolFactor2_Callback(hObject, eventdata, handles)
% hObject    handle to sliderVolFactor2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'Value') returns position of slider
%        get(hObject,'Min') and get(hObject,'Max') to determine range of slider
vol2 = get(handles.sliderVolFactor2, 'value');
set(handles.editVol2, 'String', num2str(vol2));
drawnow
guidata(hObject, handles);


% --- Executes during object creation, after setting all properties.
function sliderVolFactor2_CreateFcn(hObject, eventdata, handles)
% hObject    handle to sliderVolFactor2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: slider controls usually have a light gray background.
if isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor',[.9 .9 .9]);
end


% --- Executes on slider movement.
function sliderCorr_Callback(hObject, eventdata, handles)
% hObject    handle to sliderCorr (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'Value') returns position of slider
%        get(hObject,'Min') and get(hObject,'Max') to determine range of slider
corr_p = get(handles.sliderCorr, 'value');
set(handles.editCorr, 'String', num2str(corr_p));
drawnow
guidata(hObject, handles);

% --- Executes during object creation, after setting all properties.
function sliderCorr_CreateFcn(hObject, eventdata, handles)
% hObject    handle to sliderCorr (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: slider controls usually have a light gray background.
if isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor',[.9 .9 .9]);
end


function editMeanReverting1_Callback(hObject, eventdata, handles)
% hObject    handle to editMeanReverting1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editMeanReverting1 as text
%        str2double(get(hObject,'String')) returns contents of editMeanReverting1 as a double


% --- Executes during object creation, after setting all properties.
function editMeanReverting1_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editMeanReverting1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editMeanReverting2_Callback(hObject, eventdata, handles)
% hObject    handle to editMeanReverting2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editMeanReverting2 as text
%        str2double(get(hObject,'String')) returns contents of editMeanReverting2 as a double


% --- Executes during object creation, after setting all properties.
function editMeanReverting2_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editMeanReverting2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editVol1_Callback(hObject, eventdata, handles)
% hObject    handle to editVol1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editVol1 as text
%        str2double(get(hObject,'String')) returns contents of editVol1 as a double


% --- Executes during object creation, after setting all properties.
function editVol1_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editVol1 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editVol2_Callback(hObject, eventdata, handles)
% hObject    handle to editVol2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editVol2 as text
%        str2double(get(hObject,'String')) returns contents of editVol2 as a double


% --- Executes during object creation, after setting all properties.
function editVol2_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editVol2 (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end



function editCorr_Callback(hObject, eventdata, handles)
% hObject    handle to editCorr (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: get(hObject,'String') returns contents of editCorr as text
%        str2double(get(hObject,'String')) returns contents of editCorr as a double


% --- Executes during object creation, after setting all properties.
function editCorr_CreateFcn(hObject, eventdata, handles)
% hObject    handle to editCorr (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: edit controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end


% --- Executes on button press in test.
function test_Callback(hObject, eventdata, handles)
% hObject    handle to test (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% slider
% msgbox(sprintf('Mean Reverting Factor 1 %f', get(handles.sliderMeanRevertFactor1, 'value')),'Test')
mr1 = get(handles.sliderMeanRevertFactor1, 'value');
mr2 = get(handles.sliderMeanRevertFactor2, 'value');
vol1 = get(handles.sliderVolFactor1, 'value');
vol2 = get(handles.sliderVolFactor2, 'value');
corr_p = get(handles.sliderCorr, 'value');
msgbox({sprintf('Mean Reverting 1: %f', mr1) ...
    sprintf('Mean Reverting 2: %f', mr2)...
    sprintf('Volatility 1: %f', vol1)...
    sprintf('Volatility 2: %f', vol2)...
    sprintf('Correlation: %f', corr_p)});

%% Run the main function
% --- Executes on button press in pushbuttonRun.
function pushbuttonRun_Callback(hObject, eventdata, handles)
% hObject    handle to pushbuttonRun (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Interest rate inputs
Settle = '2015-11-30';
zeroRates = xlsread('ZeroRates11302015.xlsx', 'IR', 'C3:C62');

% G2++ parameters are in order of:
% Mean-reverting 1, mean-reverting 2, volatility 1, volatility 2, correlation
mr1 = get(handles.sliderMeanRevertFactor1, 'value');
mr2 = get(handles.sliderMeanRevertFactor2, 'value');
vol1 = get(handles.sliderVolFactor1, 'value');
vol2 = get(handles.sliderVolFactor2, 'value');
corr_p = get(handles.sliderCorr, 'value');

G2Parameters = [mr1, mr2, vol1, vol2, corr_p];

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


% % Plot the normal vol surface
% [Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
% figure
% subplot(1,2,1)
% surf(Exercise_m, Maturity_m, IRVolSurf_normal_full');
% title('IR Normal Vol Surface by Option Term and Swap Term');
% xlabel('Option Term')
% ylabel('Swap Term')
% zlabel('Normal Vol')
% view([90,30,60])
% 
% subplot(1,2,2)
% surf(Exercise_m, Maturity_m, IRVolSurf_implied_full');
% title('IR Implied Vol Surface by Option Term and Swap Term');
% xlabel('Option Term')
% ylabel('Swap Term')
% zlabel('Implied Vol')
% view([90,30,60])

handles.normal_vol_surface = IRVolSurf_normal_full;
handles.implied_vol_surface = IRVolSurf_implied_full;

which_vol = get(handles.popupmenuVolType, 'value');

if strcmp(which_vol, 'Normal Volatility')
    [Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
    surf(Exercise_m, Maturity_m, IRVolSurf_normal_full');
    %title('IR Normal Vol Surface by Option Term and Swap Term');
    xlabel('Option Term')
    ylabel('Swap Term')
    zlabel('Normal Vol')
    view([90,30,60])
else
    [Exercise_m, Maturity_m] = meshgrid(Exercise, Maturity);
    surf(Exercise_m, Maturity_m, IRVolSurf_implied_full');
    %title('IR Normal Vol Surface by Option Term and Swap Term');
    xlabel('Option Term')
    ylabel('Swap Term')
    zlabel('Normal Vol')
    view([90,30,60])   
end


% --- Executes on selection change in popupmenuVolType.
function popupmenuVolType_Callback(hObject, eventdata, handles)
% hObject    handle to popupmenuVolType (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    structure with handles and user data (see GUIDATA)

% Hints: contents = cellstr(get(hObject,'String')) returns popupmenuVolType contents as cell array
%        contents{get(hObject,'Value')} returns selected item from popupmenuVolType


% --- Executes during object creation, after setting all properties.
function popupmenuVolType_CreateFcn(hObject, eventdata, handles)
% hObject    handle to popupmenuVolType (see GCBO)
% eventdata  reserved - to be defined in a future version of MATLAB
% handles    empty - handles not created until after all CreateFcns called

% Hint: popupmenu controls usually have a white background on Windows.
%       See ISPC and COMPUTER.
if ispc && isequal(get(hObject,'BackgroundColor'), get(0,'defaultUicontrolBackgroundColor'))
    set(hObject,'BackgroundColor','white');
end
