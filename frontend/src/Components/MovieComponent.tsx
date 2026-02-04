import { Card } from "antd";
import type { MovieResponse } from "../types/moviesTypes";
import image from "../assets/image.png";

const { Meta } = Card;


function MovieComponent({movie}: {movie: MovieResponse}) {


  return (
    <div>
      <Card
        hoverable
        variant="borderless"
        cover={
          <img
            alt="movie poster"
            src={image}
          />
        }
        style={{
          textAlign: "center",
          margin: "10px"
        }}
      >
        <Meta title={movie.title} description="description"/>

      </Card>
    </div>
  )
}


export default MovieComponent;