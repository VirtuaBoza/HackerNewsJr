import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import { shallow } from 'enzyme';

describe('App', () => {
  beforeEach(() => {
    fetch.mockResponse(
      JSON.stringify([{ id: 1, title: 'title', by: 'author', url: 'url' }]),
    );
  });

  it('renders without crashing', () => {
    // Arrange
    const div = document.createElement('div');

    // Act
    // Assert
    ReactDOM.render(<App />, div);
  });

  it('Renders loading message while loading', () => {
    // Arrange
    const wrapper = shallow(<App />);

    // Act
    wrapper.setState({ loading: true });

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(1);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(0);
    expect(wrapper.find('#StoryList').length).toBe(0);
  });

  it('Renders error message if loadNewStories fails', async () => {
    // Arrange
    fetch.mockResponse(JSON.stringify({}), { status: 500 });
    const wrapper = shallow(<App />);

    // Act
    await wrapper.instance().componentDidMount();
    wrapper.update();

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(0);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(1);
    expect(wrapper.find('#StoryList').length).toBe(0);
  });

  it('Renders StoryList when loadNewStories succeeds', async () => {
    // Arrange
    const wrapper = shallow(<App />);

    // Act
    await wrapper.instance().componentDidMount();
    wrapper.update();

    // Assert
    expect(wrapper.find('#LoadingMessage').length).toBe(0);
    expect(wrapper.find('#LoadingFailMessage').length).toBe(0);
    expect(wrapper.find('#StoryList').length).toBe(1);
  });

  describe('loadNewStories', () => {
    it('sets stories and filteredStories in state', async () => {
      // Arrange
      const wrapper = shallow(<App />);
      expect(wrapper.state().stories.length).toBe(0);
      expect(wrapper.state().filteredStories.length).toBe(0);

      // Act
      await wrapper.instance().componentDidMount();
      wrapper.update();

      // Assert
      expect(wrapper.state().stories.length).toBeGreaterThan(0);
      expect(wrapper.state().filteredStories.length).toBeGreaterThan(0);
    });
  });
});
