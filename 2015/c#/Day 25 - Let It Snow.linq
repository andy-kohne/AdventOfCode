<Query Kind="Program" />

void Main()
{
	// To continue, please consult the code grid in the manual.  Enter the code at row 2947, column 3029.
	var x = FindSequenceNumber(2947, 3029);

	var startCode = 20151125;
	var code = (long)startCode;
	
	for(int i = 1; i < x; i++){
		code = GetNext(code);	
	}

	code.Dump();
}

long GetNext(long code) => (code*252533) % 33554393;

long FindSequenceNumber(int row, int col)
{
	var rowstart = 1;
	for (int r = 0; r < row; r++)
	{
		rowstart += r;
	}

	var cell = rowstart;
	for (int c = 1; c < col; c++)
	{
		cell += row + c;
	}

	return cell;
}