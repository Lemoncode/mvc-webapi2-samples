document.addEventListener('DOMContentLoaded', () => {
    document.getElementById('getdata')
        .addEventListener('click', (event) => {
            event.stopPropagation();
            fetch('http://localhost:65423/api/series')
                .then((result) => {
                    if (result.ok) {
                        return result.json();
                    }
                })
                .then((r) => console.log(r));
        });
});