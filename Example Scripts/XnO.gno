<main>
	var public cells = { "1","2","3","4","5","6","7","8","9"};
	DrawTable;
    pause;
</main>

<DrawTable>
	print << cells[0] << " | " << cells[1] << " | " << cells[2] << endl;
	print << cells[3] << " | " << cells[4] << " | " << cells[5] << endl;
	print << cells[6] << " | " << cells[7] << " | " << cells[8] << endl;
	
	print << "Choice : ";
	input public >> input
</DrawTable>