import React, { Component } from 'react';
import Header from './components/Header';
import Footer from './components/Footer';
import StoryList from './components/StoryList';
import filterObjects from './helpers/filterObjects';
import fetchJson from './helpers/fetchJson';

export default class App extends Component {
  constructor(props) {
    super(props);

    this.state = {
      stories: [],
      filteredStories: [],
      loading: true,
      loadingError: null,
      searchString: '',
    };

    this.loadNewStories = this.loadNewStories.bind(this);
    this.handleSearchChange = this.handleSearchChange.bind(this);
  }

  componentDidMount() {
    return this.loadNewStories();
  }

  async loadNewStories() {
    this.setState({ loading: true });

    try {
      const stories = await fetchJson('/api/stories/newstories/500');
      const validStories = stories.filter(
        story => story && story.by && story.title && story.url,
      );
      this.setState({
        stories: validStories,
        filteredStories: filterObjects(validStories, this.state.searchString, [
          'title',
        ]),
        loading: false,
      });
    } catch (error) {
      this.setState({ loading: false, loadingError: error });
    }
  }

  handleSearchChange(event) {
    const searchString = event.target.value;
    const filteredStories = filterObjects(this.state.stories, searchString, [
      'title',
    ]);

    this.setState({
      searchString,
      filteredStories,
    });
  }

  render() {
    return (
      <div style={{ textAlign: 'center' }}>
        <center>
          <table
            id="hnmain"
            border="0"
            cellPadding="0"
            cellSpacing="0"
            width="85%"
            bgcolor="#f6f6ef"
          >
            <tbody>
              <tr>
                <td>
                  <Header onClick={this.loadNewStories} />
                </td>
              </tr>
              <tr style={{ height: '10px' }} />
              <tr>
                <td>
                  {this.state.loading ? (
                    <div id="LoadingMessage">Loading...</div>
                  ) : this.state.loadingError ? (
                    <div id="LoadingFailMessage">
                      `This isn't going to work out, bud.`
                    </div>
                  ) : (
                    <div id="StoryList">
                      <StoryList
                        stories={this.state.filteredStories}
                        searchString={this.state.searchString}
                      />
                    </div>
                  )}
                </td>
              </tr>
              <tr>
                <td>
                  <Footer
                    searchString={this.state.searchString}
                    onChange={this.handleSearchChange}
                  />
                </td>
              </tr>
            </tbody>
          </table>
        </center>
      </div>
    );
  }
}
