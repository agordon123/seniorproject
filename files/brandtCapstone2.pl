#!/usr/bin/perl

my $input = '';
my %hash;

sub Add()
{
   my $in = undef;

   while($in ne '\n')
   {
       print "Enter Name or empty line to exit:\n";
       $in = <STDIN>;
         chomp($in);
       last if($in eq "");

         print "Enter birthday:\n";
       $birthday = <STDIN>;
     chomp($birthday);
        if($birthday eq "")
        {
            $birthday = undef;
        }
        
         $hash{$in} = $birthday;
   }

}

sub PrintAll()
{
    print "Printing all names and birthdays: \n";
    while( my ($key, $value) = each(%hash) )
    {
        print "$key\t$value\n";
    }
}


sub Search()

{
    print "Enter a name to search for: \n";
    my $in = <STDIN>;
    chomp($in);
    if(exists($hash{$in}))
    {
        if($hash{$in} ne undef)
        
        {
        print "Birthday for $in is $hash{$in}\n";
        }
        else
        {
            print "Birthday for $in is not defined\n";
        }
    }
    else
    {
        print "No person found for $in\n";
    }
}
    

sub Delete()
{
    my $in = <STDIN>;
    chomp($in);

    if(exists($hash{$in}))
    {
        delete($hash{$in});
        print "Deleted $in\n";
    }
    else
    {
        print "No person found for $in\n";
    }
}

sub Report()
{
    print "Report for all names and birthdays: \n";
    printf("%-20s %-20s\n\n", "Name", "Birthday");
    print("-"x40, "\n");

    foreach  $key (sort keys %hash)
    {

        printf("%-20s %-20s\n", "$key ","$hash{$key}");
        
    }
    print("-"x40, "\n");

}


while($input ne '6')
{
    
    
    print "Welcome to the Birthday Registry\n\n".
            "1. Add\n".
            "2. Search\n".
            "3. Print\n".
            "4. Delete\n".
            "5. Report\n".
            "6. Exit\n\n".
            "Enter your choice: \n";
            $input = <STDIN>;
            chomp($input);

        if($input eq '1')
        {
            Add();
        }
        elsif($input eq '2')
        {
            Search();
        }
        elsif($input eq '4')
        {
            Delete();
        }
         elsif($input eq '5')
        {
            Report();
        }


        if($input eq '3')
        {
            PrintAll();
        }
}