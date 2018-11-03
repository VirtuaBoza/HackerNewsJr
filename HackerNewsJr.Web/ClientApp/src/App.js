import React, { Component } from 'react';
import Header from './components/Header';
import Footer from './components/Footer';
import StoryList from './components/StoryList';

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
    this.loadNewStories();
  }

  loadNewStories() {
    this.setState({ loading: true });

    fetch('/api/stories/newstories/100')
      .then(response => {
        if (response.ok) return response.json();
        throw response;
      })
      .then(stories => {
        const validStories = stories.filter(
          story => story && story.by && story.title && story.url && story.score,
        );

        this.setState({
          stories: validStories,
          filteredStories: this.filterStories(
            validStories,
            this.state.searchString,
            ['title'],
          ),
          loading: false,
        });
      })
      .catch(error => {
        this.setState({ loading: false, loadingError: error });
        console.log(error);
      });
  }

  filterStories(validStories, searchString, fields) {
    searchString = searchString.toLowerCase();

    return validStories.filter(story => {
      let found;
      fields.forEach(field => {
        const value = story[field].toLowerCase();
        if (value.includes(searchString)) {
          found = true;
        }
      });
      return found;
    });
  }

  handleSearchChange(event) {
    const searchString = event.target.value;
    this.setState({
      searchString,
      filteredStories: this.filterStories(this.state.stories, searchString, [
        'title',
      ]),
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
                    'Loading...'
                  ) : this.state.loadingError ? (
                    `This isn't going to work out, bud.`
                  ) : (
                    <StoryList stories={this.state.filteredStories} />
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
