import React, { Component } from 'react';
import hackerNewsApi from './apis/hackerNewsApi';

export default class App extends Component {
  state = {
    stories: [],
    loading: true,
  };

  componentDidMount() {
    hackerNewsApi
      .fetch('newstories.json')
      .then(data => {
        const storyFetches = data.map(storyId =>
          hackerNewsApi.fetch(`item/${storyId}.json`),
        );
        Promise.all(storyFetches).then(stories => {
          this.setState({
            stories: stories.filter(
              story => story && story.by && story.title && story.url,
            ),
            loading: false,
          });
        });
      })
      .catch(error => {
        console.log(error);
      });
  }

  render() {
    return (
      <div>
        {this.state.loading
          ? 'Loading...'
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
