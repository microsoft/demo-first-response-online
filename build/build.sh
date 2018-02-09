function checkIfRunningFromInvalidPath
{
	RESULT=0
	WD1="`pwd`"
	pushd "`dirname \"$0\"`" > /dev/null
	WD2="`pwd`"
	popd > /dev/null

	if [ "$WD1" == "$WD2" ]; then
		RESULT=1
	fi
}

TOOLS_DIR="tools/"

checkIfRunningFromInvalidPath
if [ $RESULT == 1 ]; then
	echo "Please run this script from the root project folder."
	exit 1
fi

FAKE=$TOOLS_DIR/FAKE/tools/Fake.exe
if [ ! -f $FAKE ]; then
	echo "FAKE not found. Installing..."
	pushd $TOOLS_DIR > /dev/null
	mono nuget/nuget.exe install FAKE -ExcludeVersion -NonInteractive
	popd > /dev/null
fi

mono $FAKE $@ --nocache
