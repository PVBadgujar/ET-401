class improved_summary{
	public static void main(string[] args){
		SummaryTool summary = new SummaryTool();
		summary.init();
		summary.extractSentenceFromContext();
		summary.groupSentencesIntoParagraphs();
		summary.printSentences();
		summary.createIntersectionMatrix();

		summary.createDictionary();
		
		Console.WriteLine("SUMMMARY");
		summary.createSummary();
		summary.printSummary();

		summary.printStats();
	}
}