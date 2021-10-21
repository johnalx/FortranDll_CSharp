module mod_Fortran_dll
    use iso_c_binding
    implicit none
    
    real(c_double), parameter :: pi = 3.141592653589792
    
    contains
    
    subroutine adder(a,b,x,y)
        !DEC$ ATTRIBUTES DLLEXPORT, alias:'adder' :: adder
        !DEC$ ATTRIBUTES VALUE :: a,b
        !DEC$ ATTRIBUTES REFERENCE :: x,y
        implicit none
        integer, intent(in) :: a,b
        integer, intent(out) :: x,y
        y = a + b
        x = 2*a+3*b
    end subroutine
    
    subroutine fill_curve(n, x, y)
    !DEC$ ATTRIBUTES DLLEXPORT, alias:'fill_curve' :: fill_curve
    !DEC$ ATTRIBUTES VALUE :: n
    !DEC$ ATTRIBUTES REFERENCE :: x, y
    integer, intent(in) :: n
    real(c_double), intent(out) :: x(n), y(n)
    integer :: i
    
        x = [ ( dble(i-1), i=1,n) ]
        y = [ ( cos(pi*x(i)/10), i=1,n) ]
        
    end subroutine
    
end module