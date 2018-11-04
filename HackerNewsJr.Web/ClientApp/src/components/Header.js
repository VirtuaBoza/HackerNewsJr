import React from 'react';
import y18 from '../images/y18.gif';

const Header = props => {
  return (
    <header>
      <table
        border="0"
        cellPadding="0"
        cellSpacing="0"
        width="100%"
        style={{ padding: '2px', textAlign: 'left' }}
      >
        <tbody bgcolor="#ff6600">
          <tr>
            <td
              style={{
                width: '18px',
                paddingRight: 4,
                paddingLeft: 2,
                paddingTop: 2,
              }}
            >
              <img
                src={y18}
                width="18"
                height="18"
                style={{ border: '1px white solid', cursor: 'pointer' }}
                onClick={props.onClick}
                alt="logo"
              />
            </td>
            <td style={{ lineHeight: '12pt', height: '10px' }}>
              <span className="pagetop">
                <b
                  className="hnname"
                  style={{ paddingRight: 8, cursor: 'pointer' }}
                  onClick={props.onClick}
                >
                  Hacker News Jr.
                </b>
              </span>
            </td>
          </tr>
        </tbody>
      </table>
    </header>
  );
};

export default Header;
