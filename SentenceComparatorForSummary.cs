using System;
using System.Collections.Generic;
using System.Collections;

class SentenceComparatorForSummary  implements IComparer<Sentence>{
	
	public override int compare(Sentence obj1, Sentence obj2) {
		if(obj1.number > obj2.number){
			return 1;
		}else if(obj1.number < obj2.number){
			return -1;
		}else{
			return 0;
		}
	}
}