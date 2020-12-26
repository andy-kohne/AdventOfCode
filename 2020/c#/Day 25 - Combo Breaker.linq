<Query Kind="Statements" />

var cardPk = 9717666;
var doorPk = 20089533;

long Transform(long value, long subjectNumber) => ((value * subjectNumber ) % 20201227);
long TransformLoop(long subjectNumber, long loops) { long value=1; for (long l=0; l<loops; l++) value = Transform(value, subjectNumber); return value; }
long FindLoopCount(long publicKey) {long l = 1, v=1; while ((v = Transform(v, 7)) != publicKey) l++; return l; }

var cardLoops = FindLoopCount(cardPk);
var doorLoops = FindLoopCount(doorPk);

TransformLoop(doorPk, cardLoops).Dump();
TransformLoop(cardPk, doorLoops).Dump();
