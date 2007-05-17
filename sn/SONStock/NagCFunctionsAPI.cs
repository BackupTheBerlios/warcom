// NagCFunctionsAPI: Wrapper for NAG C Lib functions
//

using System;
using System.Text;
using System.Runtime.InteropServices; 

namespace NagCFunctionsAPI {
  // Nag Options structure

  [StructLayout(LayoutKind.Sequential)]
    public struct Nag_E04_Opt
    {
      IntPtr f_name;           // const char* 
      IntPtr f_longname;       // const char* 
      int init1;          //     Integer 
      int init2;          //    Integer 
      int minlin;      //    Nag_LinFun 
      int grade;          //    Integer 
      int nf;             //    Integer 
      int iter;           //    Integer 
      int major_iter;     //    Integer 
      public int max_iter;      //    Integer 
      int minor_max_iter;       //    Integer 
      int min_infeas;     //    Boolean 
      double f_est;           //    double 
      double step_max;        //    double 
      double step_limit;        //    double 
      double max_line_step;   //    double 
      double f_prec;          //    double 
      double linesearch_tol;  //    double 
      public double optim_tol;       //    double
      IntPtr conf;              // double *
      int nag_conf;               // Integer
      IntPtr conjac;            //    double *
      int nag_conjac;      // Integer 
      /*IntPtr r;                e04ucc */
      /* int nag_r; */
      IntPtr h;         //    double *
      int nag_h; //Integer

      int h_unit_init;    //    Boolean 
      int h_reset_freq;   //    Integer

      IntPtr s;              // double *
      int nag_s; //    Integer 
      IntPtr v;              //    double *
      int nag_v; //    Integer 
      int tdv;            //    Integer 
      IntPtr delta;          // double *
      int nag_delta; //     Integer 
      IntPtr hesl;           //     double *
      int nag_hesl;  //     Integer 
      IntPtr hesd;           //     double *
      int nag_hesd;  //     Integer 
      IntPtr state;         //     Integer *
      int nag_state;      //     Integer 
      int init_state;        //     Nag_InitType 
      int obj_deriv;      //     Boolean 
      int con_deriv;      //     Boolean 
      double f_diff_int;      //     double 
      double c_diff_int;      //     double 
      int obj_check_start;//     Integer 
      int obj_check_stop; //     Integer 
      int con_check_start;//     Integer 
      int con_check_stop; //     Integer 
      int verify_grad;  //     Nag_GradChk 
      int hessian;        //     Boolean 
      int local_search;   //     Boolean 
      int deriv_check;    //     Boolean 
      public int list;           //     Boolean 
      int print_gcheck; //     Boolean 
      int print_deriv; //     Nag_DPrintType 
      public int print_level;      //     Nag_PrintType 
      int minor_print_level; //     Nag_PrintType 
      int print_iter;     //     Integer 
      [MarshalAs (UnmanagedType.ByValArray, SizeConst=512)]
      public char [] outfile;     //     char []
      IntPtr print_fun;  //     NAG_E04_PRINTFUN 

      IntPtr ax;          //     double *
      int nag_ax;  //     Integer 
      IntPtr lambda;     //     double *
      int nag_lambda;  //     Integer 
      int fcheck;      //     Integer 
      double crash_tol;    //     double 
      int reset_ftol;  //     Integer 
      int fmax_iter;   //     Integer 
      double ftol;         //     double 
      double lin_feas_tol; //     double 
      double nonlin_feas_tol; //     double 
      int hrows;       //     Integer 
      double inf_bound;    //     double 
      double inf_step;     //     double 
      int max_df;         //     Integer 
      int  prob;    //     Nag_ProblemType 
      double rank_tol;     //     double 
      int form_hessian;  //     Boolean 
      int start;         //     Nag_Start
      int debug_iter;       //     Integer 
      int minor_debug_iter; //     Integer 
      int debug;            //     Boolean
      int used;           //     Boolean 
      int unitq;       //     Boolean 
      int nfree;       //     Integer 
      int nactiv;      //     Integer 

      /* These are used in the e04ucc stringent */
      int nprob;  //     Integer 
      int n;  //     Integer 
      int nclin;  //     Integer 
      int ncnlin;  //     Integer 
      double bndlow;  //     double 
      double bndupp;  //     double 

      /* New e04nkc members */
      int minimize;      //     Boolean 
      int factor_freq;   //     Integer 
      int partial_price; //     Integer 
      int max_sb;        //     Integer 
      double lu_factor_tol;  //     double 
      double lu_sing_tol;    //     double 
      double lu_update_tol;  //     double 
      double pivot_tol;      //     double 
      double scale_tol;      //     double 
      int crash;   //     Nag_CrashType 
      int scale;   //     Nag_ScaleType 

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=9)]
      char [] prob_name;  //     char []

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=9)]
      char [] obj_name; //     char []

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=9)]
      char [] bnd_name; //     char []

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=9)]
      char [] rhs_name; //     char []

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=9)]
      char [] range_name; //     char []

      IntPtr crnames;          //     char **
      int nag_crnames; //     Integer 

      int nsb;           //     Integer 

      /* e04nkc undocumented options */
      int max_restart;   //     Integer 
      int max_basis_len; //     Integer 
      int max_compress;  //     Integer 
      int max_basis_nfactor;  //     Integer 

      /* e04mzc options (MPS Reading) */
      double col_lo_default; //     double 
      double col_up_default; //     double 
      double infinity; //     double 

      int ncol_approx; //     Integer 
      int nrow_approx; //     Integer 
 
      double est_density; //     double 

      int output_level; //     Nag_OutputType 

      /* e04xac options */
      int deriv_want; //     Nag_DWantType 

      int use_hfwd_init; //     Boolean 
      double f_prec_used; //     double 

      /* e04ugc new members */
      int print_80ch; //     Boolean 
      int feas_exit; //     Boolean 
      int hess_freq; //     Integer 
      double elastic_wt; //     double 
      double lu_den_tol; //     double 
      int part_price; //     Integer 
      int scale_opt; //     Integer 
      int expand_freq; //     Integer 
      int hess_update; //     Integer 
      int iter_lim; //     Integer 
      double major_opt_tol; //     double 
      double minor_opt_tol; //     double 
      double unbounded_obj; //     double 
      double major_step_lim; //     double 
      int major_iter_lim; //     Integer 
      double major_feas_tol; //     double 
      int minor_iter_lim; //     Integer 
      double minor_feas_tol; //     double 
      double violation_limit; //     double 
      double nz_coef; //     double 
      int deriv_linesearch; //     Boolean 
      int hess_storage; //     Nag_HessianType 
      int direction; //     Nag_DirectionType 
      int ncon; //     Integer 
    }
  // Nag Communications structure
  public struct CommStruct
  {
    public int flag; //     Integer 
    public int first; //     Boolean   
    int last; //     Boolean    
    int nf; //     Integer      
    int it_prt; //     Boolean  
    int it_maj_prt; //     Boolean  
    int sol_sqp_prt; //     Boolean 
    int sol_prt; //     Boolean 
    int rootnode_sol_prt; //     Boolean  
    int node_prt; //     Boolean  
    int rootnode_prt; //     Boolean   
    int g_prt; //     Boolean   
    int new_lm; //     Boolean  
    int needf; //     Integer   
    int p; //     Pointer       
    IntPtr iuser; //     Integer * 
    IntPtr user; //     Integer *
    IntPtr nag_w; //     double *  
    IntPtr nag_iw; //     Integer *
    int nag_p; //     Pointer   
    IntPtr nag_print_w; //     double *
    IntPtr nag_print_iw; //     Integer *
    int nrealloc; //     Integer   
  };

  // NagError structure

  [StructLayout(LayoutKind.Sequential)]
    public struct NagError
    {
      public int code;    // int
      public int print;   // Boolean

      [MarshalAs (UnmanagedType.ByValArray, SizeConst=512)]
      public char [] char_array;    // char [NAG_ERROR_BUF_LEN]   
      public IntPtr handler;   // NAG_ERRHAN
      public int errnum;      // Integer
      public int iflag;   // Integer
      public int ival;    // Integer
    }

  // Nag Complex typ
  [StructLayout(LayoutKind.Sequential)]
    public struct Complex
    {
      public double re;
      public double im;
    };

  // Nag_QuadProgress, used by d01 quadrature routines
  [StructLayout(LayoutKind.Sequential)]
    public struct Nag_QuadProgress
    {
      int num_subint;
      int fun_count;
      IntPtr sub_int_beg_pts;
      IntPtr sub_int_end_pts;
      IntPtr sub_int_result;
      IntPtr sub_int_error;
    };

  // Nag_Spline structure used by curve fitting routines, e01, e02
  public struct Nag_Spline
  {
    int n;
    IntPtr lamda;
    IntPtr c;
    int init1;
    int init2;
  };


  // d01ajc delegate
  public delegate double NAG_D01AJC_FUN (double x);

  // e04ccc delegate
  public delegate void  NAG_E04CCC_FUN (int n, IntPtr xc_ptr, ref double fc,
                                        ref CommStruct comm);

  public delegate void  NAG_E04UCC_OBJFUN (int n, IntPtr x_ptr,  ref double objf,
                                           [In, Out] IntPtr g_ptr, ref CommStruct comm);
 
  public delegate void  NAG_E04UCC_CONFUN (int n, int ncnlin, IntPtr needc_ptr,
                                           IntPtr x_ptr, [In, Out] IntPtr conf_ptr,
                                           [In, Out] IntPtr conjac_ptr,
                                           ref CommStruct comm);
 

  public  enum Nag_OrderType { Nag_RowMajor = 101, Nag_ColMajor = 102 };

  public enum  Nag_JobType {
    Nag_DoBoth=1044,
    Nag_EigVals,
    Nag_DoNothing,
    Nag_Permute,
    Nag_Schur,
    Nag_Scale,
    Nag_Subspace,
    Nag_EigVecs
  } ;
 
 
  public enum  Nag_UploType
    {
      Nag_Upper = 121,
      Nag_Lower = 122 };
 
  public class NagFunctions {
   
   
   
    [DllImport(@"nagc.dll")]
      public static extern void d01ajc(NAG_D01AJC_FUN f , double a, double b, 
                                       double epsabs, double epsrel, int max_num_subint,
                                       ref double result, ref double abserr,
                                       ref Nag_QuadProgress qp,
                                       ref NagError fail);
 
    [DllImport("nagc")]
      public static extern void e01bac(int m, double [] x, double [] y,
                                       ref Nag_Spline spline, ref NagError fail);

    [DllImport("nagc")]
      public static extern void e02bbc(double x, ref double s, ref Nag_Spline spline,
                                       ref NagError fail);


    [DllImport("nagc")]
      public static extern void e04ccc(int n, NAG_E04CCC_FUN funct, double [] x, 
                                       ref double fmin, ref Nag_E04_Opt options,
                                       ref CommStruct user_comm, ref NagError fail);

    [DllImport("nagc")]
      public static extern void e04ucc(int n, int nclin, int ncnlin, double [,] a,
                                       int tda, double [] bl, double [] bu,
                                       NAG_E04UCC_OBJFUN objfun,
                                       NAG_E04UCC_CONFUN confun,
                                       double [] x, ref double objf, double [] g,
                                       ref Nag_E04_Opt options,
                                       ref CommStruct user_comm,
                                       ref NagError fail);



    [DllImport("nagc")]
      public static extern void f02aac (int n, double [,] a, int tda, double [] r,
                                        ref NagError fail);

    [DllImport("nagc")]
      public static extern void  f08fqc( Nag_OrderType order, Nag_JobType  job,
                                         Nag_UploType uplo, int n, 
                                         [In, Out] Complex [,] a, int pda, double [] w, ref NagError fail); 

    [DllImport("nagc")]
      public static extern void g02dac(/* Nag_IncludeMean */ int mean, int n,
                                       double [,] x, int  tdx, int m, int [] sx, int ip,
                                       double [] y, double [] wt, ref double rss,
                                       ref double df, double [] b, double [] se,
                                       double [] cov, double [] res, double [] h,
                                       double [,] q, int tdq, ref int svd, ref int rank,
                                       double [] p, double tol, double [] comm_ar,
                                       ref NagError fail);

    [DllImport("nagc")]
      public static extern void g03aac(int /*Nag_PrinCompMat*/ pcmatrix,
                                       int /*Nag_PrinCompScores*/ scores,
                                       int n, int m, double [,] x,  int tdx,
                                       int [] isx,  double [] s,
                                       double [] wt,  int nvar,  double [,] e,  int tde,
                                       double [,] p,  int tdp,  double [,] v,  int tdv,
                                       ref NagError fail);


    [DllImport("nagc")]
      public static extern void g03eac(/*Nag_MatUpdate */ int update,
                                       /* Nag_DistanceType */ int dist,
                                       /* Nag_VarScaleType*/ int scale,
                                       int n, int m, double [,] x,  int tdx,
                                       int [] isx, double [] s, double [] d,
                                       ref NagError fail);

    [DllImport("nagc")]
      public static extern void g03ecc(/* Nag_ClusterMethod */ int method, int n,
                                       double [] d,  int [] ilc, int [] iuc,
                                       double [] cd,  int [] iord, double [] dord,
                                       ref NagError fail);

    [DllImport("nagc")]
      public static extern void g03efc(int n, int m,  double [,] x, int tdx, int [] isx,
                                       int nvar, int k, double [,] cmeans,  int tdc,
                                       double [] wt, int [] inc, int [] nic,
                                       double [] css, double [] csw,  int maxit,
                                       ref NagError fail);



    [DllImport("g03ehcdll")]
      public unsafe static extern void g03ehc(/*Nag_DendOrient*/int orient,  int n,
                                              double [] dord, double dmin,
                                              double dstep,  int nsym,  /*ref IntPtr c*/
                                              out IntPtr c, ref NagError fail);

    [DllImport("nagc")]
      public static extern void g03xzc( ref IntPtr c /* char ***c */);



    [DllImport("nagc")]
      public static extern void g05cbc(int seed);

    [DllImport("nagc")]
      public static extern int g05eyc(IntPtr r);

    [DllImport("nagc")]
      public static extern void g05ecc(double t, ref IntPtr r, ref NagError fail);


    [DllImport("nagc")]
      public static extern void e04xxc(ref Nag_E04_Opt options);

    [DllImport("nagc")]
      public static extern string x04mcc(int enum_int, string enum_string);


      [DllImport("nagc.dll")]
      public unsafe static extern  void g13ffc(int num, int nt, int p, int q,
                          double[] theta, double gamma, double[] fht, double[] ht,
                          double[] et, ref NagError fail);
  }


}
