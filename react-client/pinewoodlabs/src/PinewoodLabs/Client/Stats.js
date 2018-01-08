import React from 'react';
import { LineChart } from 'react-chartkick';
window.Highcharts = require('highcharts');

class Stats extends React.Component {
    constructor(props) {
        super(props);
        this.socket = props.socket;
        this.state = {
            data: []
        };
    }
    componentDidMount() {
        this.socket.on('sendStats', function (data) {
            this.setState({
                data: data,
            });

        }.bind(this));
    }

    render() {
        return (
            <div id="stats">
                <header>
                    <h1>Speed Over Time</h1>
                </header>
                <LineChart
                    width="700px"
                    height="340px"
                    colors={["#FF8400", "#FF8400"]}
                    legend={false}
                    xtitle="Time" 
                    ytitle="Speed"
                    library={{plotOptions: {
                        series: {
                            animation: false
                        }
                    },}}
                    data={this.state.data}
                />
            </div>
        );
    }
}

export default Stats;