function handleClick() {
}
function handleClickFavorite() {
    alert('Элемент добавлен в избранное')
}
function applyPriceFilter() {
    var minPrice = document.getElementById("minPrice").value;
    var maxPrice = document.getElementById("maxPrice").value;

    // Создаем новый объект XMLHttpRequest
    var xhr = new XMLHttpRequest();

    // Указываем метод запроса и URL
    xhr.open("GET", "/Home/FilterByPrice?minPrice=" + minPrice + "&maxPrice=" + maxPrice, true);

    // Определяем обработчик события onload
    xhr.onload = function () {
        // Проверяем, успешен ли запрос
        if (xhr.status >= 200 && xhr.status < 300) {
            // Обновляем содержимое страницы с помощью полученных данных
            document.getElementById("resultContainer").innerHTML = xhr.responseText;
        } else {
            // В случае ошибки выводим сообщение об ошибке
            console.error("Ошибка при выполнении запроса: " + xhr.statusText);
        }
    };

    // Отправляем запрос
    xhr.send();
}
