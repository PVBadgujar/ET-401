using System;
using System.Collections.Generic;
using System.Collections;

class SentenceComparator  implements IComparer<Sentence>{
	
	public override int compare(Sentence obj1, Sentence obj2) {
		if(obj1.score > obj2.score){
			return -1;
		}else if(obj1.score < obj2.score){
			return 1;
		}else{
			return 0;
		}
	}
}