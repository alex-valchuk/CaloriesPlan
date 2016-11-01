appModule.filter('friendlyBooleanText', function () {
	return function (input) {

		if (input && input === true) {
			return 'Yes';
		}

		return 'No';
	}
});

appModule.filter('friendlyBooleanStyle', function () {
    return function (input) {

        if (input && input === true) {
            return 'green-row';
        }

        return 'red-row';
    }
});

appModule.filter('utcToLocal', function ($filter) {
    return function (utcDateString, format) {
        // return if input date is null or undefined
        if (!utcDateString) {
            return;
        }

        // append 'Z' to the date string to indicate UTC time if the timezone isn't already specified
        if (utcDateString.indexOf('Z') === -1 && utcDateString.indexOf('+') === -1) {
            utcDateString += 'Z';
        }

        // convert and format date using the built in angularjs date filter
        return $filter('date')(utcDateString, format);
    }
});
