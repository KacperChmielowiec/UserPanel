const getFetchApi = (url) => {


    const baseURL = url?.length > 0 ? url : `${window.location.protocol}//${window.location.host}`

    const cleanSlug = (slug) => {
        return slug.startsWith('/') ? slug.slice(1) : slug;
    };

    const fetchGet = async (slug, headers = {}, queryParams = {}) => {
        
        const queryString = Object.keys(queryParams).length ? ["?", new URLSearchParams(queryParams).toString()].join("") : "";
        const slugClear = cleanSlug(slug);

        const fullStringPath = `${baseURL}/${slugClear}${queryString}`;

        try {
            const response = await fetch(fullStringPath, {
                method: 'GET',
                headers: {
                    'Content-Type': 'application/json',
                    ...headers
                }
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error during fetch GET:', error);
            throw error;
        }
    }

    const fetchPost = async (slug, body, headers = {}, queryParams = {}) => {

        const queryString = Object.keys(queryParams).length ? ["?", new URLSearchParams(queryParams).toString()].join("") : "";
        const slugClear = cleanSlug(slug);
        
        const fullStringPath = `${baseURL}/${slugClear}${queryString}`;

        console.log("fullPath", fullStringPath)
        console.log("body", body)
        try {
            const response = await fetch(fullStringPath, {
                method: 'POST',
                cache: "no-cache",
                credentials: "same-origin",
                headers: {
                    'Content-Type': 'application/json',
                    ...headers
                },
                referrerPolicy: "no-referrer",
                body: JSON.stringify(body)
            });
            if (!response.ok) {
                throw new Error(`HTTP error! Status: ${response.status}`);
            }
            const data = await response.json();
            return data;
        } catch (error) {
            console.error('Error during fetch POST:', error);
            throw error;
        }
    }

    return {
        get: fetchGet,
        post: fetchPost,
    }

}

