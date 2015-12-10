function enable_parallel_processing(varargin)
% Use the number of cores specified.  If no number is specified we query the
% machine to get the number of cores available.
if ~nargin
	n_core = feature('numcores');
	
else
	n_core = varargin{1};
	
end

% Enable parallel processing.  This is used to speed up 'parfor' loops in
% the code.
% distcomp.feature('LocalUseMpiexec', false);
try 
    matlabpool('open', 'local', n_core);
    disp('Parallel Computing for Multiple Cores has been enabled.');

catch err
    % Check if the error is caused by another interative session is open.
    % We already have pool open and don't need to do it again.
    if strcmp(err.identifier, 'parallel:cluster:MatlabpoolRunValidation')
        disp('Parallel Computing for Multiple Cores has already been enabled.');
        
    else
		rethrow(err);
		
    end
        
end

end