import React from 'react';
import { Stage, Layer, Rect, Circle, Line } from 'react-konva';

class Editor extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            socket: props.socket,
            maxPoints: 40,
            fixedPoints: 2,
            points: [],
            lines: [],
            width: 700,
            height: 260,
            margin: 20,
            wheel1x: 522,
            wheel2x: 81,
            wheelOffset: 40
        };
    }
    componentWillMount() {
        const pointA = <RacerNode x={this.state.wheel1x + this.state.margin} y={this.state.height + this.state.margin - this.state.wheelOffset} color="#F05858" key={0} />
        const pointB = <RacerNode x={this.state.wheel2x + this.state.margin} y={this.state.height + this.state.margin - this.state.wheelOffset} color={"#F05858"} key={1} />

        this.setState({
            points: [pointA, pointB]
        });
    }

    handleAddPoint(e) {
        if (this.state.points.length >= this.state.maxPoints) {
            return;
        }
        const position = e.currentTarget.getPointerPosition();
        const points = this.state.points.concat(
            [<RacerNode
                x={position.x}
                y={position.y}
                color="#589EF0"
                draggable={false}
                onDragMove={this.handlePointDragMove}
                onDragEnd={this.handlePointDragEnd}
                key={this.state.points.length}
            />]
        );
        this.setState({
            points: points
        });
        this.updateLines(points);
        this.sendToClient(points);
    }

    handleUndo() {
        if (this.state.points.length > this.state.fixedPoints) {
            const points = this.state.points.slice();
            points.pop();
            this.setState({
                points: points
            });
            this.updateLines(points);
            this.sendToClient(points);
        }
    }

    sendToClient(points) {
        const simple_points = points.map((point) => {
            return ({
                x: ((point.props.x - (this.state.width / 2.0)) / 1000.0),
                y: ((-point.props.y + (this.state.height / 2.0)) / 1000.0),
            });
        });
        this.state.socket.emit('updateCar', simple_points);
    }

    updateLines(points) {
        if (!points) {
            points = this.state.points.slice();
        }
        const lines = points.map((point, i) => {
            var next_i = i + 1;
            if (next_i === points.length) {
                next_i = 0;
            }
            return (<RacerLine
                start={point}
                end={this.state.points[next_i]}
                color="#FF8400"
                width={5}
            />);
        });
        this.setState({
            lines: [lines]
        })
    }

    startRace() {
        this.state.socket.emit('startRace', null);
    }

    render() {
        return (
            <div id="editor">
                <header>
                    <h1>Design your Pinewood Racecar</h1>
                </header>
                <Stage width={this.state.width + (this.state.margin * 2.0)} height={this.state.height + (this.state.margin * 3.0)} onClick={this.handleAddPoint.bind(this)} >
                    <Layer>
                        <Rect
                            x={0}
                            y={0}
                            width={this.state.width + (this.state.margin * 2.0)}
                            height={this.state.height + (this.state.margin * 3.0)}
                            fill="#FFFFFF"
                        />
                        <Rect cornerRadius={10}
                            x={this.state.margin}
                            y={this.state.margin}
                            width={this.state.width}
                            height={this.state.height - this.state.wheelOffset}
                            fill="#AAAAAACC"
                        />
                        <Circle
                            x={this.state.wheel1x + this.state.margin}
                            y={this.state.height + this.state.margin - this.state.wheelOffset}
                            radius={60}
                            fill="#666666CC"
                        />
                        <Circle
                            x={this.state.wheel2x + this.state.margin}
                            y={this.state.height + this.state.margin - this.state.wheelOffset}
                            radius={60}
                            fill="#666666CC"
                        />
                        {this.state.lines}
                        {this.state.points}
                    </Layer>
                </Stage>
                <div>
                    <p>Click in the box to shape your Pinewood Derby racecar.</p>
                    <button className="editor" onClick={this.handleUndo.bind(this)}>Undo</button>
                    <button className="editor" onClick={this.startRace.bind(this)}>Race it!</button>
                </div>
            </div>

        );
    }
}

class RacerNode extends React.Component {
    constructor(props) {
        super(props);

        this.state = {
            x: props.x,
            y: props.y,
            color: props.color,
        };
    }

    render() {
        return (
            <Circle
                x={this.state.x}
                y={this.state.y}
                radius={10}
                fill={this.state.color}
            />
        );
    }
}

function RacerLine(props) {
    return (
        <Line
            points={[
                props.start.props.x,
                props.start.props.y,
                props.end.props.x,
                props.end.props.y
            ]}
            dash={[33, 10]}
            lineCap="round"
            stroke={props.color}
            strokeWidth={props.width}
        />
    );
}

export default Editor;