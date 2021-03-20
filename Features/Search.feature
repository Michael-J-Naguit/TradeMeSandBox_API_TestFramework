Feature: Search
	Trade Me Search Requests Tests

@mytag
Scenario: Search "product" in in category Antiques & Collectables > Stamps
	Given Setup General Search request
	And Setup General Search request parameters
		| Category   | Item    |
		| 0187-4383- | product |
	When Send request to API
	Then Response status is 'OK'