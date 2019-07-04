# Gnosis

Gnosis is an intepreter for a language Gnos. It is currently in development and is being written purley in c#.

Gnos makes use of a unique syntax. This is shown in the example below:

    <main>
        print << "Hello World!!" << " I am genos reborn" << endl;
        input "What is your name? " >> name;
        print << "Hello " << name << endl;
        pause;
    </main>


There are 5 variable types: double, float, long, int, bool and string. Variable declearation and use is dynamic. The "var" keyword is used to initialize variables which are decided automatically by the intepreter. This is displayed in the example below

    <main>
        var value = 4 + 5 + 6;
        var value2 = value + 1;
        var str = "\nHello strings work too " + value2 + " Not in a strict way either";

        print << value + " " + value2<< endl;
        print << str << endl;

        pause;
    </main>
    
   
