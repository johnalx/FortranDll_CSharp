# Test project for calling Fortran code from CSharp

This project is in response to SO question https://stackoverflow.com/questions/69664946/calling-fortran-from-c-sharp-in-vs2019-with-ifort

## Fortran Project

I am using Intel Fortran oneAPI 2021 for compiling the F90 code.

I am currently declaring two subroutines inside a module. Once to test read/write of integer values called `adder` and the next to test read/write of arrays called `fill_curve`.

```fortran
    subroutine adder(a,b,x,y)
        !DEC$ ATTRIBUTES DLLEXPORT, alias:'adder' :: adder
        !DEC$ ATTRIBUTES VALUE :: a,b
        !DEC$ ATTRIBUTES REFERENCE :: x,y
        implicit none
        integer, intent(in) :: a,b
        integer, intent(out) :: x,y

    end subroutine
    
    subroutine fill_curve(n, x, y)
    !DEC$ ATTRIBUTES DLLEXPORT, alias:'fill_curve' :: fill_curve
    !DEC$ ATTRIBUTES VALUE :: n
    !DEC$ ATTRIBUTES REFERENCE :: x, y
    integer, intent(in) :: n
    real(c_double), intent(out) :: x(n), y(n)

    end subroutine
```

## CSharp Project

I am using .NET 5 for compiling the CS code.

The driver code calls the Fortran dll functions exported above with the declarations below:

```cs
    [DllImport("FortranLib.dll", EntryPoint = "adder")]
    static extern void adder(int a, int b, [Out] out int x, [Out] out int y);

    [DllImport("FortranLib.dll", EntryPoint = "fill_curve")]
    static extern void fill_curve(
        int n, 
        double[] x, 
        double[] y);

```

care must be taken to include the .dll from the fortran compilation in the same folder as the csharp binaries. I have done this by including a link to an existing item ("Add Existing Item, Add As Link") of executable type and setting the property to copy on compile.
I have done this using the recommendations from https://devopsonwindows.com/handle-platform-specific-dependencies/ and I specified
a project property that takes the value of `..\FortranLib\Debug` on x86 and `..\FortranLib\x84\Debug` on x64, in order to
always link the correct dll in the csharp project.