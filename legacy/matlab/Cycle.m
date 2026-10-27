function Cycle(Inp,rule)
global Net WInp WNet Tau Lambda SzNet Con Noise Naturaleza Signo Ganma

% Membrane potential
%InpPot=OutInp(WInp'*Inp);
InpPot=WInp'*Inp+Noise*(2*rand(SzNet,1)-1);

%positive noise [0..0.1]
Net(:,1)=Out(InpPot+WNet'*Net(:,2));

switch lower(rule)
	case {'pre-not-post'}
		Pre=Net(:,2)';
		Pos=Net(:,1);
      
%      WNet= WNet+(Ganma-abs(WNet)).*Con.*(Pos*(Pre-Tau));	%pre-not-post
      WNet= WNet+(Ganma-abs(WNet)).*Con.*(Pos*(Pre-Tau));	%pre-not-post
      
	case {'post-not-pre'}
		Pre=Net(:,2);
		Pos=Net(:,1)';
      
		WNet= WNet+(Ganma-abs(WNet)).*Con.*(Pre*(Pos-Tau));	%post-not-pre
      
   case {'brunel'}
		Pre=Net(:,2);
		Pos=Net(:,1)';
      
		WNet= WNet+Con.*(Pre*(Pos.*(Pre-Tau)'.*(Pos-Tau)));	% Brunel
             
   case {'bcm'}
		Pre=Net(:,2);
		Pos=Net(:,1)';
      
		WNet= WNet+Con.*(Pre*(Pos.*(Pos-Tau)));	% BCM
             
   case {'ba'}
		Pre=Net(:,2);
		Pos=Net(:,1)';
      
		WNet= WNet+Con.*(Pre*(Pos.*(Pos-Tau).*(Lambda-Pos)));	% BA
             
   case {'bah'}
      Pre=Net(:,2);
      Pre=Pre.*(Pre>Tau);
      Pos=Net(:,1)';
      
		WNet= WNet+Con.*(Pre*(Pos.*(Pos-Tau).*(Lambda-Pos)));	% BAh
             
   case {'cba'}
      Pro=Net(:,2)*Net(:,1)';
      
		WNet= WNet+Con.*(Pro.*(Pro-Tau).*(Lambda-Pro));	% CBA
             
	otherwise
		disp('Unknown method.')
end

if (Naturaleza)
	WNet=(1-(Signo==-sign(WNet))).*WNet;
end
   
% Output value
Net(:,2)=Net(:,1);

