
#!/usr/bin/perl

#im on windows does that even do anything?
#over engineered? defintely. absolutely gross program. 

open my $shapes, "<", "shapes.dat";

$regex = "^(.+?):";
my %functions = ('circle' => 'circle', 'circumference'=> 'circumference' ,'triangle' => 'triangle', 'rectangle' => 'rectangle' , 'trapezoid' => 'trapezoid');


sub circle( )
{
    my $func = shift;

        if($func =~ $regex)
        {
         my $res = $1;
         
            if($functions{$res}){
             
                  $functions{$res}->("$'");
            }
        }
    

}

sub circumference()
{
    
    my $res = shift;
    # couldn't find pi nice without using libraray, so heres a solid value

    my $pi = 3.14159265358979;
    if($res =~ /radius=/)
    {
       
        my $rad = $';
        chomp($rad);
        print "Circle Radius: $rad, Circumfrence: ". (2 * $pi * $') . "\n";
      
    }
}

sub triangle()
{
    my ($base,$height) = 0;
    # the fact that i managed to even come up with this is a testament to my ability to learn this god aweful language
    my $reg = "(?<==)(.*?)(?=:|\n)";
my $func = shift;
    if($func =~ $regex)
    {
        my $res = $1;
        if($res eq "area")
        {
            $res = $';
            if($res =~ $reg)
            {
                $base = $&;
                $res = $';
                if($res =~ $reg)
                {
                    $height = $&;
                    print "Triangle Base: $base, Height: $height, Area: " . ($base * $height) / 2 . "\n";
                }
            }
        }

    }
}


sub rectangle()
{

    my ($length, $width) = 0;
    my $reg = "(?<==)(.*?)(?=:|\n)";
    my $func = shift;

    if($func =~ $regex)
    {
     my $res = $1;
        if($res eq "perimeter")
        {
           $res = $';
           if($res =~ $reg)
           {
               $width = $&;
               $res = $';
               if($res =~ $reg)
               {
                   $length = $&;
                   print "Rectangle Length: $length, Width: $width, Perimeter: " . ($length * 2 + $width * 2) . "\n";
               }
            }
        }
    }
}

sub trapezoid()
{
    my($baseA, $baseB, $height) = 0;
    my $reg = "(?<==)(.*?)(?=:|\n)";
    #need this one cause the EOF is not a newline
    my $reg2 = "(?<=height=)(.*?)(?=\$)";
    my $func = shift;

    if($func =~ $regex)
    {
        my $res = $1;
        if($res eq "area")
        {
            
            $res = $';
            if($res =~ $reg)
            {
                
                $baseA = $&;
                $res = $';
                
                if($res =~ $reg)
                {
                    
                    $baseB = $&;
                    $res = $';
                   
                    if($res =~ $reg2)
                    {
                        $height = $&;
                        print "Trapezoid BaseA: $baseA, BaseB: $baseB, Height: $height, Area: " . (($baseA + $baseB) / 2 * $height) . "\n";
                    }
                    
                }
            }
        }
    }
}


sub print_shapes
{
    while(<$shapes>)
    {
        if($_ =~ $regex)
        {
            my $result = $&;
            # the regex includes the delimited :, i dont want it so chop chop
            chop($result);
            #print "$result $'\n";
            if($functions{$result})
            {
                # call the function, i googled this
               
                $functions{$result}->("$'");
            }
       

        
        }
     
    }
}




print_shapes();