const MorrisUtils = () => {

    const extractChartDataVisit = (data, keyString) => {
        let payload = [];
        const labels = [];
        data.forEach((campaning, i) => {
            if (campaning[keyString].length == 0) return
            labels.push(campaning.name)
            campaning[keyString].forEach((item, index) => {
                if (payload.length <= index)
                    payload.push({ date: item.date })
                payload[index] = { ...payload[index], [campaning.name]: item.value }
            })
        })
        payload = { data: payload, labels: labels }
        return payload
    }
    const extractChartDataClicks = (data, keyString) => {
        let payload = [];
        data.forEach((campaning, i) => {
            if (campaning[keyString].length == 0) return
            campaning[keyString].forEach((item, index) => {
                if (payload.length <= i)
                    payload.push({ label: campaning.name, value: 0 })
                payload[i] = { ...payload[i], value: Number(payload[i].value += item.value) }
            })
        })
        return payload
    }
    const extractChartDataBudget = (data, keyString) => {
        let payload = [];
        const labels = [];
        const budgetList = []
        data.forEach((campaning, i) => {
            if (campaning[keyString].length == 0) return
            labels.push(campaning.name)
            campaning[keyString].forEach((item, index) => {
                if (payload.length <= index)
                    payload.push({ date: item.date })
                budgetList.push(item.value)
                payload[index] = { ...payload[index], [campaning.name]: item.value }
            })
        })
        payload = {
            data: payload,
            labels: labels,
            max: Math.max(...budgetList),
            min: Math.min(...budgetList)
        }
        return payload
    }

    const loadVisitChart = (dataVisit, tag = "") => {

        const selector = document.querySelector(tag);
        if(!selector) throw new Error("Morriss urils [loadVisitChart] did not find tag")
        document.querySelector(tag).innerHTML = '';
        if (dataVisit == null) return
        Morris.Bar({
            element: 'visit',
            data: dataVisit.data,
            xkey: 'date',
            ykeys: dataVisit.labels,
            labels: dataVisit.labels
        }).on('click', function (i, row) {
            console.log(i, row);
        });
    }
    const loadClicksChart = (dataClicks, tag = "") => {
        const selector = document.querySelector(tag);
        if (!selector) throw new Error("Morriss urils [loadVisitChart] did not find tag")
        if (dataClicks == null) return
        Morris.Donut({
            element: 'clicks',
            data: dataClicks,
            formatter: function (x) { return x + " clicks" }
        }).on('click', function (i, row) {
            console.log(i, row);
        });
    }
    const loadBudgetChart = (dataBudget, tag = "") => {
        const selector = document.querySelector(tag);
        if (!selector) throw new Error("Morriss urils [loadVisitChart] did not find tag")
        window.m = Morris.Line({
            element: 'budget',
            data: dataBudget.data,
            xkey: 'date',
            ykeys: dataBudget.labels,
            labels: dataBudget.labels,
            parseTime: false,
            goals: [dataBudget.min, (dataBudget.min + dataBudget.max) / 2, dataBudget.max]
        });
    }


    return {
        extractChartDataVisit: extractChartDataVisit,
        extractChartDataClicks: extractChartDataClicks,
        extractChartDataBudget: extractChartDataBudget,
        loadVisitChart: loadVisitChart,
        loadClicksChart: loadClicksChart,
        loadBudgetChart: loadBudgetChart
    }
}

const tabUtils = (configuration) => {

    if (typeof configuration !== 'object') return;

    var tabWrapper = $(configuration.tabWrapper),
        tabMnu = tabWrapper.find(configuration.tabItems),
        tabContent = tabWrapper.find(configuration.tabContents);

    tabContent.not(':first-child').hide();
    tabMnu.first().addClass('active');
    tabMnu.each(function (i) {
        $(this).attr('data-tab', 'tab' + i);
    });
    tabContent.each(function (i) {
        $(this).attr('data-tab', 'tab' + i);
    });

    tabMnu.click(function () {
        var tabData = $(this).data('tab');
        tabWrapper.find(tabContent).hide();
        tabWrapper.find(tabContent).filter('[data-tab=' + tabData + ']').show();
    });

    $(configuration.tabItems).click(function () {
        var before = $(configuration.tabItems + ".active");
        before.removeClass('active');
        $(this).addClass('active');
    });
}


const modalUtils = () => {

    let currentOpen = ""

    const modal = document.querySelector('.modal'),
        title = document.querySelector(".modal-title"),
        body = document.querySelector(".modal-body"),
        closeButtons = document.querySelectorAll('.close-modal'),
        openButtons = document.querySelectorAll('.open-modal'),
        submitButtons = document.querySelectorAll('.submit-modal');

    function openModal() {
        modal.classList.add("transition");
        modal.classList.add('visible')
        setTimeout(() => modal.classList.remove("transition"), 350)
    }

    function closeModal() {
        modal.classList.add("transition");
        modal.classList.remove('visible')
        currentOpen = "";
        setTimeout(() => modal.classList.remove("transition"), 350)
    }

    openButtons.forEach((b, i) => {
        let index = i + 1;
        let formattedIndex = index < 10 ? `0${index}` : index;
        b.dataset.id = `b-${ formattedIndex }-${ Math.floor(1000 + Math.random() * 9000) }`;
    })

    return {
        closeModalButtons: closeButtons,
        openModalButtons: openButtons,
        open: openModal,
        close: closeModal,
        body: body,
        setTemplate: (template) => {
            if (template?.title && title) {
                title.innerHTML = template.title
            }
            if (template?.body && body) {
                body.innerHTML = template.body
            }
        },
        setOpenModal: (id,callback) => {
            openButtons?.forEach(b => {
                if (b.dataset.id === id) {
                    b.addEventListener("click", () => { currentOpen = b.dataset.id; callback() })
                }
            })
        },
        setSubmit: (id, callback) => {
            submitButtons.forEach((b) => {
                b.addEventListener("click", () => { if(currentOpen === id) callback(modal)  })
            })
        }
    }
}

const modalTemplates = () => {
    return {
        createAd: {
            title: "Wybierz format reklamy",
            body: `<div style="width: 500px">
                    <form class="" >
                        <label  class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Wybierz format:</label>
                        <select name="template" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                            <option value="Static">Statyczna reklama</option>
                            <option value="Dynamic">Dynamiczna reklama</option>
                        </select>
                    </form>
                    </div>`
        },
        createFeed: {
            title: "Dodaj Feed",
            body: `<div style="width: 500px">
                    <form class="">
                        <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Wybierz format:</label>
                        <select name="format" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500">
                            <option value="Ceneo">Ceneo</option>
                        </select>
                        </br>
                        <label class="block mb-2 text-sm font-medium text-gray-900 dark:text-white">Wprowadz Url:</label>
                        <input name="url" type="text" id="helper-text" aria-describedby="helper-text-explanation" class="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5  dark:bg-gray-700 dark:border-gray-600 dark:placeholder-gray-400 dark:text-white dark:focus:ring-blue-500 dark:focus:border-blue-500" placeholder="adres url">
                    </form>
                    </div>`
        }
    }
}