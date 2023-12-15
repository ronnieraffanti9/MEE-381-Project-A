using System;

public class PendSim : Simulator
{
    double L;
    double k; // Spring constant
    double m; // Mass
    double g; // Acceleration due to gravity
    double l_0; // Natural length of the spring

    public PendSim() : base(6)
    {
        k = 90.0;
        m = 1.4;
        g = 9.81;
        l_0 = 0.9;

        x[0] = -0.7; // default x
        x[1] = 0.0; // default v_x
        x[2] = -0.2; // default y
        x[3] = 0.0; // default v_y
        x[4] = 0.5; // default z
        x[5] = 0.9; // default v_z
        
        SetRHSFunc(RHSFuncPendulum);
    }
//----------------------------------------------------
// RHSFuncPendulum
//----------------------------------------------------
    private void RHSFuncPendulum(double[] xx, double t, double[] ff)
    {
        // Unpack variables for clarity
        double u1 = xx[0]; // x
        double u2 = xx[1]; // v_x
        double u3 = xx[2]; // y
        double u4 = xx[3]; // v_y
        double u5 = xx[4]; // z
        double u6 = xx[5]; // v_z

        // Compute L and spring force
        double L = Math.Sqrt(u1 * u1 + (u3 - 1.2) * (u3 - 1.2) + u5 * u5);
        double springForce = k * (L - l_0);

        // Equations of motion
        ff[0] = u2; // dx/dt = v_x
        ff[1] = (-springForce * u1) / (L * m); // dv_x/dt
        ff[2] = u4; // dy/dt = v_y
        ff[3] = ((-springForce * u3) / (L * m)) - g ; // dv_y/dt
        ff[4] = u6; // dz/dt = v_z
        ff[5] = (-springForce * u5) / (L * m); // dv_z/dt
    }

    public float xCoord{
        get{
            return((float)x[0]);
        }
    }
    public float yCoord{
        get{
            return((float)x[2]);
        }
    }

    public float zCoord{
        get{
            return((float)x[4]);
        }
    }
    public double kE{
        get{
            double velocity = Math.Sqrt(x[1]*x[1] + x[3]*x[3] + x[5]*x[5]);
            return(.5 * m * velocity * velocity);
        }
    }
    public double pE{
        get{
            double L = Math.Sqrt(x[0]*x[0] + (x[2] - 1.2)*(x[2] - 1.2) + x[4]*x[4]);
            double deltaL = Math.Abs(L - l_0);
            double springPotental = deltaL * deltaL * k  * .5;
            double gravityPotental = m * g * x[2];
            return(springPotental + gravityPotental);
        }
    }
    public double totalE{
        get{
            return(pE + kE);
        }
    }
}