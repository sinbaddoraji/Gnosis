<main>
    input >> ra;
	
    var dumper = 0;
    var size = $ra - 1;
	
	print << size << endl;
	
    for(var i = 0| i < size|i++)
	{
        var man = ra[i];
		if (man == "a"){dumper++;};
		if (man == "e"){dumper++;};
		if (man == "i"){dumper++;};
		if (man == "u"){dumper++;};
		if (man == "o"){dumper++;};
    };
    print << dumper << endl;
    pause;
</main>