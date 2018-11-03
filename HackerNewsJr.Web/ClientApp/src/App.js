import React, { Component } from 'react';

export default class App extends Component {
  state = {
    stories: [],
    loading: true,
    loadingError: null,
  };

  componentDidMount() {
    fetch('/api/stories/newstories/5')
      .then(response => {
        if (response.ok) return response.json();
        throw response;
      })
      .then(stories => {
        this.setState({
          stories: stories.filter(
            story => story && story.by && story.title && story.url,
          ),
          loading: false,
        });
      })
      .catch(error => {
        this.setState({ loading: false, loadingError: error });
        console.log(error);
      });
  }

  render() {
    return (
      <div>
        {this.state.loading
          ? 'Loading...'
          : this.state.loadingError
            ? `This isn't going to work out, bud.`
            : this.state.stories.map(story => (
                <div key={story.id}>
                  <h1>
                    <a href={story.url}>{story.title}</a>
                  </h1>
                  <p>{story.by}</p>
                  <br />
                </div>
              ))}
      </div>
    );
  }
}
