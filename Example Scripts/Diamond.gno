<main>
    var zama = "    ";
    var coie = "*";
    var b = 0;
    while(b < 5){
        print << zama << coie << endl;
        remove(zama,0,1);
        coie += "**";
        b++;
    };
	
    var lai = "*******";
    var space = " ";
    int a = 0;
    while(a < 4){
        print << space << lai << endl;
        remove(lai,0,2);
        space += " ";
        a++;
    };
	pause;
</main>
