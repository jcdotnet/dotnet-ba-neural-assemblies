#include <math.h>
#include <string.h>
#include <stdlib.h>

#include "tarn.H"
     

     float rand_float(float min, float max) { 
         return (min + ((max - min)*(float)rand() / RAND_MAX)); 
     }   		
	
	
 // constructor 
	Tarn::Tarn (char *aprend,const int entr,const int neu){
         int i,j;
         // almacenamos los parametros en las vbles miembro 
         SzInp=entr; SzNet=neu;
         rule=strdup(aprend);
         // reserva de memoria para las matrices (uso malloc de C) 
         // new no se si se puede asignar a arrays bidimensionales
         WInp= (float **) malloc(SzInp* sizeof(float*));
            for (i=0;i<SzInp;i++)
               WInp[i]=(float*) malloc(SzNet*sizeof(float)); 
         WNet= (float **) malloc( sizeof(float*)*SzNet);
            for (i=0;i<SzNet;i++)
              WNet[i]=(float*) malloc(SzNet*sizeof(float)); 
         net = (float **) malloc( sizeof(float*)*SzNet);
            for (i=0;i<SzNet;i++)
              net[i]=(float*) malloc(2*sizeof(float));
         // inicializacion de las matrices
          for (i=0;i<SzNet;i++) {  
             net[i][0]=0;       
             net[i][1]=0;	
             for (j=0;j<SzNet;j++)   
                WNet[i][j]=0;
         }
         for (i=0;i<SzInp;i++) { 
               for (j=0;j<SzNet;j++) 
                    WInp[i][j]=0;
         }
    }      
    
       
	
    // metodo que conecta la red (da valores a los pesos de las conexiones)
	void Tarn::conectar() {
         int i,n,ind,temp,NbExc;
       	 int *aux, *aux2;
       	 float Mn;         	    
           	    
         // valores por defecto de los pesos segun el aprendizaje
         if ((strcmp(rule,"pre-not-post")==0) ||(strcmp(rule,"post-not-pre")==0)) 
  	     {        MnW[0]=0.2; MnW[1]=0.02; MnW[2]=-0.03; } 
         else if (strcmp(rule,"bcm")==0) {  	               
  	  			  MnW[0]=0.2;  MnW[1]=0.02; MnW[2]=-0.03;  }                       
	     else if (strcmp(rule,"ba")==0) {
  	    		  MnW[0]=0.2;  MnW[1]=0.02; MnW[2]=-0.03; 
       	 }		
         Mn=MnW[1]; 
         
         // nivel de ruido en las conexiones
         NsW[0]=0.25*MnW[0];
         NsW[1]=0.25*MnW[1];
               
         //numero de neuronas excitadoras
         NbExc=(PcExc*SzNet)/100;   
                 
         // Asociamos pesos aleatorios a las conexiones                               
         for (n=0;n<SzNet;n++) {   // hasta NbExc excitadoras,demas inhib
              if (n==NbExc)
                  Mn=MnW[2];      
              // pesos conex hacia-adelante
              aux= new int[SzInp];
              for (i=0;i<SzInp;i++)
                  aux[i]=i;
              for (i=0;i<SzInp;i++) {
                  ind= rand()%SzInp;
                  temp=aux[i];aux[i]=aux[ind];aux[ind]=temp;
              }                                                    
              for (i=0;i<NbInp;i++)    
                  WInp[aux[i]][n]=MnW[0]+(NsW[0]*(2.0*rand_float(0,1.0)-1.0));                                
              // pesos conex laterales     
              aux2= new int[SzNet];
              for (i=0;i<SzNet;i++)             
                  aux2[i]=i;
              for (i=0;i<SzNet;i++) {
                   ind= rand()%SzNet;
                   temp=aux2[i];aux2[i]=aux2[ind];aux2[ind]=temp;
              } 
              for (i=0;i<NbExc;i++) {
                   if (aux2[i]==n) aux2[i]=aux2[NbExc];    // quita autoconexion
              }
              for (i=0;i<NbLat;i++)                     // conexiones laterales
                   WNet[n][aux2[i]]=Mn+(NsW[1]*(2.0*rand_float(0,1.0)-1.0));
         }  
         delete [] aux;
         delete [] aux2;                    
	}  // end conectar()
		
  
	// metodo q enseña un patron a la red
    // pat-> patron que mostramos
    // inic-> se inicializa la actividad de la red a 0
    // almac:0 se deja la red como estaba,1 se adapta a la conectividad (aprend)
    // ite- num it. q se repite la presentacion
    // pasos 0 se deja converger hasta la estabilidad, !0 num pasos fijo
    // lim, si pasos=0 valor umbral de la actividad para considerar red estable 
	void Tarn::show(int pat[], int inic,int almac,int ite,int pasos,float lim) {
         int i,j,idIte,seguir;
         float **W0;        
         
         for (idIte=1;idIte<=ite;idIte++) {
            if (inic==0) {               // inicial=0 red a cero
                for (i=0;i<SzNet;i++) {      
                     net[i][0]=0; net[i][1]=0;	
                }
            }
            if (almac==0){               // almac=0 salvo estado de la red
                W0= (float**)malloc( sizeof(float*)*SzNet); 
                for (i=0;i<SzNet;i++) 
                  W0[i]=(float*)malloc(SzNet*sizeof(float));               
                for (i=0;i<SzNet;i++){
                  for (j=0;j<SzNet;j++)
                     W0[i][j]=WNet[i][j];
                }                      
            }
            if (pasos==0) {              // empieza dinamica de la red
                seguir=1;
                while (seguir) {                      	      	  
               	    cycle(pat);                             	    
               	    seguir=0;
               	    for (i=0;i<SzNet;i++) {
               	        if ((0.5-fabs(net[i][1]-0.5))>lim) { 
               	             seguir=1;break;  }        
                   	}                   	   
                }
            
             }else {
                for (i=1;i<=pasos;i++)
                    cycle(pat);
                }                     
                     
             if (almac==0) {             // almac=0 recupero estado de la red
                 for (i=0;i<SzNet;i++){
                   for (j=0;j<SzNet;j++)
                        WNet[i][j]=W0[i][j];
                 }
                 for (i=0;i<SzNet;i++) 
                    free(W0[i]);  
                 free(W0);             
             }                                     
         }
    }
		
   	
    void  Tarn::cycle(int inp[]) {               
        int i,j,k;           
        float *inpPot,*aux,*pre,*pos,sum,vAbs;
        float sft=0.5,slp=10.0; 

        inpPot= new float[SzNet];
        aux= new float[SzNet];
        pre= new float[SzNet];
        pos= new float[SzNet];
        
        // inpPot->entrada neuronas (sumatorio entrada*pesos de WInp)         
        for (k=0;k<SzNet;k++) {
          inpPot[k]=0;
          for(i=0;i<SzInp;i++)
             inpPot[k]+=inp[i]*WInp[i][k];
        }         
        // aux -> entrada total de las neuronas
        for (k=0;k<SzNet;k++) {
          aux[k]=0;
          for (i=0;i<SzNet;i++) 
              aux[k]+=net[i][1]*WNet[i][k];               
          aux[k]+=inpPot[k];
        }              
        delete [] inpPot;
        // funcion de transferencia para obtener la nueva salida d la red                
        for (i=0;i<SzNet;i++) {
            net[i][0]=1.0/(1.0+exp(slp*((-aux[i])+sft)));
        }           
        
        // calculo nuevos valores de los pesos (aprendizaje)
        for (i=0;i<SzNet;i++) {
  	         pre[i]=net[i][1];
  	         pos[i]=net[i][0];
        }                
        tau=0.5;
        if (strcmp(rule,"pre-not-post")==0)  { 	              
  	       alfa=0.05; con=alfa; ganma=0.1;               
           for (i=0;i<SzNet;i++) {
  	         for (j=0;j<SzNet;j++) 
  	          WNet[i][j]=WNet[i][j]+(ganma-fabs(WNet[i][j]))*con*(pos[i]*(pre[j]-tau));
  	       }} 	               
  	     else if (strcmp(rule,"post-not-pre")==0) {
  	       alfa=0.05; con=alfa; ganma=0.1;
  	       for (i=0;i<SzNet;i++) {
  	         for (j=0;j<SzNet;j++)
  	          WNet[i][j]=WNet[i][j]+(ganma-fabs(WNet[i][j]))*con*(pre[i]*(pos[j]-tau));
  	       }} 	        
         else if (strcmp(rule,"bcm")==0) {
           alfa=0.02; con=alfa;
           for (i=0;i<SzNet;i++)
  	           aux[i]=pos[i]*(pos[i]-tau);
  	       for (i=0;i<SzNet;i++) {
  	         for (j=0;j<SzNet;j++)
  	           WNet[i][j]=WNet[i][j]+con*(pre[i]*aux[j]);
  	       }} 	
         else if (strcmp(rule,"ba")==0) {
  	       alfa=0.05; con=alfa; lambda=1.0;
           for (i=0;i<SzNet;i++)
  	           aux[i]=pos[i]*(pos[i]-tau)*(lambda-pos[i]);
  	       for (i=0;i<SzNet;i++) {
  	         for (j=0;j<SzNet;j++)
  	           WNet[i][j]=WNet[i][j]+(pre[i]*aux[j]*con); 	               
  	       }} 
             
         delete []aux; delete []pre; delete[] pos;
         // actualizamos salida de la red
         for (i=0;i<SzNet;i++)
             net[i][1]=net[i][0];        
	}	

     // destructor
     Tarn::~Tarn() {
        int i;
        for (i=0;i<SzNet;i++)
            free(WInp[i]); 
        free(WInp);
        for (i=0;i<SzNet;i++)
            free(WNet[i]);  
        free(WNet);
        for (i=0;i<2;i++)
           free(net[i]);  
        free(net);
     }

