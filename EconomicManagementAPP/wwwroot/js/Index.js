function initializeTransactionsForm(urlGetCategories) {

    $("#OperationTypeId").change(async function () {
        const valueSelect = $(this).val();
        const response = await fetch(urlGetCategories, {
            method: 'POST',
            body: valueSelect,
            headers: {
                'Content-Type': 'application/json'
            }
        });
        const json = await response.json();
        console.log(json);
        const options = json.map(category => `<option value =${category.value}>${category.text}</option>`);
        $("#CategoryId").html(options);

    })
}