function N=Out(I);
global Sft Slp

N=1.0./(1.0+exp(Slp.*(-I+Sft)));
