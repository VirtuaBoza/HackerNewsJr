const hackerNewsApi = {
  fetch(endpoint, options) {
    return window
      .fetch(`https://hacker-news.firebaseio.com/v0/${endpoint}`, options)
      .then(response => {
        if (response.ok) return response.json();
        throw response;
      });
  },
};

export default hackerNewsApi;
