// ARQUITECTURA RED NEURONAL CON APRENDIZAJE 
// APRENDIZAJES Q SOPORTA: pre-not-post, post-not-pre, bcm, ba

#ifndef TARN_H
#define TARN_H

class Tarn {
	 
	  // ************* declaracion de las variables de la red neuronal
	  #define PcExc 70       // porcentaje neuronas excitadoras
      #define NbInp 20       // numero de conexiones hacia-adelante
      #define NbLat 10       // numero de conexiones laterales
      int SzInp;     // tamaño de entrada de la red
      int SzNet;     // numero de neuronas de la red
      float **WInp;  // matriz pesos de las conex. hacia-adelante
      float **WNet;  //  matriz pesos de las conexiones laterales 
      float MnW[3],NsW[3];    // pesos de las conexiones 
      float alfa,ganma,lambda,tau,con;  // ctes de aprendizaje
      char *rule;   // regla de aprendizaje de la red
    
      public:
        float **net;
   
         
    // *************** declaracion de metodos  ******************** 
    private:
		void cycle(int[]); 
	
	public:
      // constructor:RECIBE LOS PARAMETOS DE LA RED
	  // aprend: aprendizaje que queremos para la red
	  // entr: tamaño de la capa de entrada
	  // neu: tamaño de la salida: numero de neuronas
	  Tarn(char *aprend,const int entr,const int neu);		   
 
	  // destructor
      ~Tarn();
           
      // metodo que conecta la red (da valores a los pesos de las conexiones)
      void conectar();
		
      // metodo q enseña un patron a la red
      // pat-> patron que mostramos
      // inic-> se inicializa la actividad de la red a 0
      // almac:0 se deja red como estaba,1 se adapta a la conectividad(aprend)
      // ite- num it. q se repite la presentacion
      // pasos 0 se deja converger hasta la estabilidad, !0 num pasos fijo
      // lim, si pasos=0 valor umbral d la actividad para considerar red estable 
      void show(int pat[], int inic,int almac,int ite,int pasos,float lim);		        
};

#endif



   
