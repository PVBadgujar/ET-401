using System;
using System.Collections.Generic;
using System.Collections;

class Paragraph{
	int number;
	ArrayList<Sentence> sentences;

	Paragraph(int number){
		this.number = number;
		sentences = new ArrayList<Sentence>();
	}
}