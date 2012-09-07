jmeter-toolkit
==============

JMeter Chart Builder

Tool to create charts from JMeter logs.

Usage

Commands:

ProcessFile: generate chart based on a single log file

Options:

--file: input file

ProcessDirectory: generate chart(s) based on the files contained in the input directory

	Options:
	
		--dir: input directory
		
		--outDir: output directory
		
		--merge: yes will merge all the input files, and based on the data, produce a comparison chart, or trend chart. Note that the tool will produce a trend chart if requests are made at different dates.

Types of charts

Comparison chart

	Comparison chart shows the results of all the requests contained in the test plan.

	The chart shows the following information:

	- The average response time
	- The min/max response time
	- The average time excluding the 10% slowest requests, and the 10% fastest requests
	- The min/max response time excluding the 10% slowest requests, and the 10% fastest requests
	- The distribution of the response time

Trend chart

	The trend chart shows the evolution of the response time for a specific request over a period of time. Each bar represents the average response time for a specific date.

	- The average response time
	- The min/max response time
	- The average time excluding the 10% slowest requests, and the 10% fastest requests
	- The min/max response time excluding the 10% slowest requests, and the 10% fastest requests
	- The distribution of the response time