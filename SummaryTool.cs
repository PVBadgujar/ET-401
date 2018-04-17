using System;
using System.Collections.Generic;

internal class SummaryTool
{
	internal System.IO.FileStream @in;
	internal System.IO.FileStream @out;
	internal List<Sentence> sentences, contentSummary;
	internal List<Paragraph> paragraphs;
	internal int noOfSentences, noOfParagraphs;

	internal double[][] intersectionMatrix;
	internal LinkedHashMap<Sentence, double> dictionary;


	internal SummaryTool()
	{
		@in = null;
		@out = null;
		noOfSentences = 0;
		noOfParagraphs = 0;
	}

	internal virtual void init()
	{
		sentences = new List<Sentence>();
		paragraphs = new List<Paragraph>();
		contentSummary = new List<Sentence>();
		dictionary = new LinkedHashMap<Sentence, double>();
		noOfSentences = 0;
		noOfParagraphs = 0;
		try
		{
			@in = new System.IO.FileStream("samples/amazon/nexus_6p", System.IO.FileMode.Open, System.IO.FileAccess.Read);
			@out = new System.IO.FileStream("output.txt", System.IO.FileMode.Create, System.IO.FileAccess.Write);
		}
		catch (FileNotFoundException e)
		{
			Console.WriteLine(e.ToString());
			Console.Write(e.StackTrace);
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			Console.Write(e.StackTrace);
		}
	}

	/*Gets the sentences from the entire passage*/
	internal virtual void extractSentenceFromContext()
	{
		int nextChar, j = 0;
		int prevChar = -1;
		try
		{
			while ((nextChar = @in.Read()) != -1)
			{
				j = 0;
				char[] temp = new char[100000];
				while ((char)nextChar != '.')
				{
					//System.out.println(nextChar + " ");
					temp[j] = (char)nextChar;
					if ((nextChar = @in.Read()) == -1)
					{
						break;
					}
					if ((char)nextChar == '\n' && (char)prevChar == '\n')
					{
						noOfParagraphs++;
					}
					j++;
					prevChar = nextChar;
				}

				sentences.Add(new Sentence(noOfSentences,(new string(temp)).Trim(),(new string(temp)).Trim().Length,noOfParagraphs));
				noOfSentences++;
				prevChar = nextChar;
			}
		}
		catch (Exception e)
		{
			Console.WriteLine(e.ToString());
			Console.Write(e.StackTrace);
		}

	}

	internal virtual void groupSentencesIntoParagraphs()
	{
		int paraNum = 0;
		Paragraph paragraph = new Paragraph(0);

		for (int i = 0;i < noOfSentences;i++)
		{
			if (sentences[i].paragraphNumber == paraNum)
			{
				//continue
			}
			else
			{
				paragraphs.Add(paragraph);
				paraNum++;
				paragraph = new Paragraph(paraNum);

			}
			paragraph.sentences.add(sentences[i]);
		}

		paragraphs.Add(paragraph);
	}
internal virtual double noOfCommonWords(Sentence str1, Sentence str2)
{
		double commonCount = 0;

		foreach (string str1Word in str1.value.Split("\\s+"))
		{
			foreach (string str2Word in str2.value.Split("\\s+"))
			{
				if (str1Word.compareToIgnoreCase(str2Word) == 0)
				{
					commonCount++;
				}
			}
		}

		return commonCount;
}

	internal virtual void createIntersectionMatrix()
	{

		intersectionMatrix = RectangularArrays.ReturnRectangularDoubleArray(noOfSentences, noOfSentences);
		for (int i = 0;i < noOfSentences;i++)
		{
			for (int j = 0;j < noOfSentences;j++)
			{

				if (i <= j)
				{
					Sentence str1 = sentences.get(i);
					Sentence str2 = sentences.get(j);
					intersectionMatrix[i][j] = noOfCommonWords(str1,str2) / ((double)(str1.noOfWords + str2.noOfWords) / 2);
				}
				else
				{
					intersectionMatrix[i][j] = intersectionMatrix[j][i];
				}

			}
		}
	}

	internal virtual void createDictionary()
	{
		for (int i = 0;i < noOfSentences;i++)
		{
			double score = 0;
			for (int j = 0;j < noOfSentences;j++)
			{
				score += intersectionMatrix[i][j];
			}
			dictionary.put(sentences.get(i), score);
			((Sentence)sentences.get(i)).score = score;
		}
	}

	internal virtual void createSummary()
	{

		  for (int j = 0;j <= noOfParagraphs;j++)
		  {
				  int primary_set = paragraphs.get(j).sentences.size() / 5;

				  //Sort based on score (importance)
				  Collections.sort(paragraphs.get(j).sentences,new SentenceComparator());
				  for (int i = 0;i <= primary_set;i++)
				  {
					  contentSummary.add(paragraphs.get(j).sentences.get(i));
				  }
		  }

		  //To ensure proper ordering
		  Collections.sort(contentSummary,new SentenceComparatorForSummary());

	}


	internal virtual void printSentences()
	{
		foreach (Sentence sentence in sentences)
		{
			Console.WriteLine(sentence.number + " => " + sentence.value + " => " + sentence.stringLength + " => " + sentence.noOfWords + " => " + sentence.paragraphNumber);
		}
	}

	internal virtual void printIntersectionMatrix()
	{
		for (int i = 0;i < noOfSentences;i++)
		{
			for (int j = 0;j < noOfSentences;j++)
			{
				Console.Write(intersectionMatrix[i][j] + "    ");
			}
			Console.Write("\n");
		}
	}

	internal virtual void printDicationary()
	{
		  // Get a set of the entries
		  ISet<object> set = dictionary.entrySet();
		  // Get an iterator
		  System.Collections.IEnumerator i = set.GetEnumerator();
		  // Display elements
		  while (i.MoveNext())
		  {
			 System.Collections.IDictionary.Entry me = (System.Collections.IDictionary.Entry)i.Current;
			 Console.Write(((Sentence)me.Key).value + ": ");
			 Console.WriteLine(me.Value);
		  }
	}

	internal virtual void printSummary()
	{
		Console.WriteLine("no of paragraphs = " + noOfParagraphs);
		foreach (Sentence sentence in contentSummary)
		{
			Console.WriteLine(sentence.value);
		}
	}

	internal virtual double getWordCount(List<Sentence> sentenceList)
	{
		double wordCount = 0.0;
		foreach (Sentence sentence in sentenceList)
		{
			wordCount += (sentence.value.Split(" ")).length;
		}
		return wordCount;

internal static class RectangularArrays
{
    internal static double[][] ReturnRectangularDoubleArray(int size1, int size2)
    {
        double[][] newArray = new double[size1][];
        for (int array1 = 0; array1 < size1; array1++)
        {
            newArray[array1] = new double[size2];
        }

        return newArray;
    }
}
internal virtual void printStats()
{
		Console.WriteLine("number of words in Context : " + getWordCount(sentences));
		Console.WriteLine("number of words in Summary : " + getWordCount(contentSummary));
		Console.WriteLine("Commpression : " + getWordCount(contentSummary) / getWordCount(sentences));
}


}

