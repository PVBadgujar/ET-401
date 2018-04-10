class Sentence{
	int paragraphNumber;
	int number;
	int stringLength; //Dont need this 
	double score;
	int noOfWords;
	string value;

	Sentence(int number, string value, int stringLength, int paragraphNumber){
		this.number = number;
		this.value = new string(value);
		this.stringLength = stringLength;
		noOfWords = value.split("\\s+").length;
		score = 0.0;
		this.paragraphNumber=paragraphNumber;
	}
}