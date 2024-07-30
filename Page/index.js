const skippedProperties = ['isUpToUp', 'dartType', 'menu', 'skills', 'damInfo'];

document.addEventListener('DOMContentLoaded', function () {
    const allSideMenu = document.querySelectorAll('#sidebar .side-menu.top li a');
    const tableContainer = document.querySelector('#table-container');
    let currentData = null;

    allSideMenu.forEach(item => {
        const li = item.parentElement;
        item.addEventListener('click', function () {
            allSideMenu.forEach(i => {
                i.parentElement.classList.remove('active');
            });
            li.classList.add('active');
            loadPageContent(item.dataset.page);
        });
    });

    function loadPageContent(page) {
        let url;
        let title;
        let breadcrumb;
        let searchContainer = '';
        switch (page) {
            case 'itemTemplates':
                url = 'Server1/ItemTemplates.json';
                title = 'Item Templates';
                breadcrumb = 'Item Templates';
                break;
            case 'npcTemplates':
                url = 'Server1/NpcTemplates.json';
                title = 'NPC Templates';
                breadcrumb = 'NPC Templates';
                searchContainer = `
                    <div class="search-container">
                        <input type="text" id="search-npc" placeholder="Tìm kiếm theo tên NPC...">
                        <button onclick="searchData()">Tìm kiếm</button>
                    </div>`;
                break;
            case 'nClasses':
                url = 'Server1/NClasses.json';
                title = 'Class Skills';
                breadcrumb = 'Class Skills';
                searchContainer = `
                    <div class="search-container">
                        <input type="text" id="search-class" placeholder="Tìm kiếm theo tên lớp...">
                        <button onclick="searchData()">Tìm kiếm</button>
                    </div>`;
                break;
            case 'itemOptions':
                url = 'Server1/ItemOptionTemplates.json';
                title = 'Item Options';
                breadcrumb = 'Item Options';
                searchContainer = `
                    <div class="search-container">
                        <input type="text" id="search-id" placeholder="Tìm kiếm theo số thứ tự...">
                        <input type="text" id="search-name" placeholder="Tìm kiếm theo tên...">
                        <button onclick="searchData()">Tìm kiếm</button>
                    </div>`;
                break;
            case 'maps':
                url = 'Server1/Maps.json';
                title = 'Maps';
                breadcrumb = 'Maps';
                searchContainer = `
                    <div class="search-container">
                        <input type="text" id="search-id" placeholder="Tìm kiếm theo số thứ tự...">
                        <input type="text" id="search-name" placeholder="Tìm kiếm theo tên...">
                        <button onclick="searchData()">Tìm kiếm</button>
                    </div>`;
                break;
        }
        document.querySelector('#page-title').innerText = title;
        document.querySelector('#breadcrumb-active').innerText = breadcrumb;
        
        fetchData(url, page, searchContainer);
    }

    function fetchData(url, page, searchContainer) {
        fetch(url)
            .then(response => response.json())
            .then(data => {
                if (page === 'nClasses') {
                    let data2;
                    data.forEach(item => {
                        if (!data2) 
                            data2 = item.skillTemplates;
                        else 
                        data2 = data2.concat(item.skillTemplates);
                    });
                    data = data2;
                    data.sort((a, b) => a.id - b.id);
                }
                currentData = data;
                let tableData = `<table id="data-table"><thead><tr>`;
                Object.getOwnPropertyNames(currentData[0]).forEach(prop => {
                    if (skippedProperties.includes(prop))
                        return;
                    tableData += `<th>${prop.charAt(0).toUpperCase() + prop.slice(1)}</th>`;
                });
                tableData += `</tr></thead><tbody></tbody></table>`;
                tableContainer.innerHTML = searchContainer + tableData;
                displayData(data, page);

                if (page === 'npcTemplates') {
                    document.querySelector('#search-npc').addEventListener('input', searchData);
                } else if (page === 'nClasses') {
                    document.querySelector('#search-class').addEventListener('input', searchData);
                } else if (page === 'itemOptions') {
                    document.querySelector('#search-id').addEventListener('input', searchData);
                    document.querySelector('#search-name').addEventListener('input', searchData);
                } else if (page === 'maps') {
                    document.querySelector('#search-id').addEventListener('input', searchData);
                    document.querySelector('#search-name').addEventListener('input', searchData);
                }
            })
            .catch(error => console.error('Error loading data:', error));
    }

    function displayData(data, page) {
        const tableBody = document.querySelector('#data-table tbody');
        tableBody.innerHTML = '';
        data.forEach(item => {
            const dataRow = document.createElement('tr');
            Object.getOwnPropertyNames(item).forEach(prop => {
                if (skippedProperties.includes(prop))
                    return;
                dataRow.innerHTML += `<th>${item[prop]}</th>`;
            });
            tableBody.appendChild(dataRow);
        });
    }

    function searchData() {
        const searchClass = document.getElementById('search-class') ? document.getElementById('search-class').value.toLowerCase() : '';
        const searchNpc = document.getElementById('search-npc') ? document.getElementById('search-npc').value.toLowerCase() : '';
        const searchID = document.getElementById('search-id') ? document.getElementById('search-id').value : '';
        const searchName = document.getElementById('search-name') ? document.getElementById('search-name').value.toLowerCase() : '';

        let filteredData = currentData;

        if (searchClass && document.querySelector('#search-class')) {
            filteredData = filteredData.filter(item => item.name.toLowerCase().includes(searchClass));
        }

        if (searchNpc && document.querySelector('#search-npc')) {
            filteredData = filteredData.filter(item => item.name.toLowerCase().includes(searchNpc));
        }

        if (searchID && document.querySelector('#search-id')) {
            filteredData = filteredData.filter(item => item.id.toString() === searchID);
        }

        if (searchName && document.querySelector('#search-name')) {
            filteredData = filteredData.filter(item => item.name.toLowerCase().includes(searchName));
        }

        displayData(filteredData, document.querySelector('#page-title').innerText.toLowerCase().replace(' ', ''));

        if (filteredData.length === 0) {
            alert('No results found');
        }
    }

    loadPageContent('itemTemplates');
});