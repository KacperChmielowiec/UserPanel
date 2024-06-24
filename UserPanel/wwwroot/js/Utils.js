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