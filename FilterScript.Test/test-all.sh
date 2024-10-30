#!/bin/bash
declare -i RESULT=0
for f in Scripts/*.txt
do
	echo -e "$f \c"
	dotnet ../FilterScript/bin/Release/net8.0/FilterScript.dll -s $f -i "Images/_input.bmp" -o "${f}.bmp"	
	dotnet bin/Release/net8.0/FilterScript.Test.dll "Images/$(basename -- $f).bmp" "${f}.bmp"
	if [ $? = 0 ]; then
		echo "OK"
	else
		echo "ERROR"
		RESULT=1
	fi
done

exit $RESULT
